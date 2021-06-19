using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Apollo.Data;
using Apollo.Models;
using Microsoft.EntityFrameworkCore;

namespace Apollo.Services
{
    public class SongService
    {
        private readonly DataContext _context;
        public SongService(DataContext context)
        {
            _context = context;
        }

        public ArrayList GetMatchingSongs(string str)
        {
            if(String.IsNullOrEmpty(str))
            {
                return new ArrayList();
            }

            var strToLower = str.ToLower();
            // using include is the only way to connect the database object of one model to another
            var matchingSongs = _context.Song.Where(s => s.Title.ToLower().Contains(strToLower))
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .Include(s => s.Category)
                .ToList();

            ArrayList matchingSongsList = new ArrayList();

            foreach(Song song in matchingSongs)
            {
                var album = "";

                if (song.Album != null)
                {
                    album = song.Album.Title;
                }
                else
                {
                    album = null;
                }

                matchingSongsList.Add(new
                {
                    id = song.Id,
                    title = song.Title,
                    category = song.Category.Name,
                    artist = song.Artist.StageName,
                    album = album,
                    length = song.Length,
                    plays = song.Plays,
                    rating = song.Rating,
                    releaseDate = song.ReleaseDate
                });
            }

            return matchingSongsList;
        }

        public ArrayList filterSongs(string str)
        {
            var strToLower = str.ToLower();
            
            var matchingSongs = _context.Song
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .Include(s => s.Category)
                .Where(s => s.Title.ToLower().Contains(strToLower) ||
                            s.Category.Name.ToLower().Contains(strToLower) ||
                            s.Artist.StageName.ToLower().Contains(strToLower) ||
                            s.Plays.ToString().ToLower().Contains(strToLower) ||
                            s.Rating.ToString().ToLower().Contains(strToLower) ||
                            s.Length.ToString().ToLower().Contains(strToLower) ||
                            s.ReleaseDate.ToString().ToLower().Contains(strToLower) ||
                            s.Album.Title.ToLower().Contains(strToLower))
                .ToList();

            ArrayList matchingSongsList = new ArrayList();

            foreach (Song song in matchingSongs)
            {
                var album = "";

                if (song.Album != null)
                {
                    album = song.Album.Title;
                }
                else
                {
                    album = null;
                }

                matchingSongsList.Add(new
                {
                    id = song.Id,
                    title = song.Title,
                    category = song.Category.Name,
                    artist = song.Artist.StageName,
                    album = album,
                    length = song.Length,
                    plays = song.Plays,
                    rating = song.Rating,
                    releaseDate = song.ReleaseDate
                });
            }

            return matchingSongsList;
        }
    }
}
