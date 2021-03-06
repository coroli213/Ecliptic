﻿using Ecliptic.Models;
using Ecliptic.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Ecliptic.Views.UserInteraction;
using Xamarin.Forms;
using static Ecliptic.Views.UserInteraction.Authorization;
using Xunit;
using Ecliptic.Data;

namespace EclipticTests.DatabaseTests
{
    [TestClass]
    public class Remove
    {
        [TestCleanup()]
        public void MyTestCleanup()
        {
            // обновить базу данных
            DbService.ClearAll();
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            DbService.ClearAll();
            DbService.RefrashDb(true);
        }

        [TestMethod]
        public void RemoveOne()
        {
            // Arrange   -------------------------------------    
            Note note = new Note("заметка", "213", "KSU", false);
            DbService.AddNote(note);

            // Act   -----------------------------------------        
            DbService.RemoveNote(note);

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void RemoveEmpty()
        {
            // Arrange   -------------------------------------    
            Note note = new Note("заметка", "213", "KSU", false);
            DbService.AddNote(note);

            // Act   -----------------------------------------      
            Note empnote = null;
            DbService.RemoveNote(empnote);

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void RemoveMany()
        {
            // Arrange   -------------------------------------
            DbService.AddNote(new Note("I'm open note", "213", "KGU", true));
            DbService.AddNote(new Note("I'm open okey", "213", "KGU", true));
            DbService.AddNote(new Note("I'm open yesi", "522", "KGU", true));
            DbService.AddNote(new Note("I'm open noby", "231", "KGU", true));
            DbService.AddNote(new Note("I'm open puko", "409", "KGU", true));
            var notes = DbService.LoadAllNotes();

            // Act   -----------------------------------------
            DbService.RemoveNote(notes);

            // Assert-----------------------------------------
            int count = DbService.LoadAllNotes().Count;

            Assert.AreEqual(0, count);
        }
    }
}
