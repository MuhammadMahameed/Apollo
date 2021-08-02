using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apollo.Data;
using Apollo.Models;
using Nancy.Json;

namespace Apollo.Controllers
{
    public class VotesController : Controller
    {
        private readonly DataContext _context;

        public VotesController(DataContext context)
        {
            _context = context;
        }

        public async Task Create(string type, int recordId, string username, int score)
        {
            if (!CheckVoteExists(type,recordId,username))
            {
                Vote vote = new Vote();
                vote.Type = type;
                vote.RecordId = recordId;
                vote.Username = username;
                vote.Score = score;

                _context.Add(vote);
                await _context.SaveChangesAsync();

                // get all votes to this record
                var recordVotes = _context.Vote.Where(x => x.Type == type &&
                                                           x.RecordId == recordId)
                                                .ToList();

                dynamic record;

                if (type == "song")
                    record = _context.Song.FirstOrDefault(x => x.Id == recordId);
                else
                    record = _context.Album.FirstOrDefault(x => x.Id == recordId);

                // calculate the avg rating score
                var sum = 0D;

                foreach (Vote recordVote in recordVotes)
                    sum += recordVote.Score;

                sum /= recordVotes.Count;
                sum = double.Parse(string.Format("{0:0.00}", sum));
                record.Rating = sum;

                _context.Update(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Edit(string type, int recordId, string username, int score)
        {
            if (CheckVoteExists(type,recordId, username))
            {
                var vote = _context.Vote.FirstOrDefault(x => x.Type == type &&
                                                             x.RecordId == recordId &&
                                                             x.Username == username);
                vote.Score = score;

                _context.Update(vote);
                await _context.SaveChangesAsync();

                // get all votes to this record
                var recordVotes = _context.Vote.Where(x => x.Type == type &&
                                                           x.RecordId == recordId)
                                                .ToList();

                dynamic record;

                if (type == "song")
                    record = _context.Song.FirstOrDefault(x => x.Id == recordId);
                else
                    record = _context.Album.FirstOrDefault(x => x.Id == recordId);

                // calculate the avg rating score
                var sum = 0D;

                foreach (Vote recordVote in recordVotes)
                    sum += recordVote.Score;

                sum /= recordVotes.Count;
                sum = double.Parse(string.Format("{0:0.00}", sum));
                record.Rating = sum;

                _context.Update(record);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Vote>> GetUserVotes(string type, string username)
        {
            return await _context.Vote.Where(x => x.Type == type && x.Username == username).ToListAsync();
        }

        public bool CheckVoteExists(string type, int recordId, string username)
        {
            return _context.Vote.Any(x => x.Type == type &&
                                          x.RecordId == recordId &&
                                          x.Username == username);
        }
    }
}
