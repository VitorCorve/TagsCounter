using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace TagsCounter.Models.Services
{
    public class FileManager
    {
        public static IEnumerable<string> ReadSelectedFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;

            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    return File.ReadLines(openFileDialog.FileName);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }
        public static ObservableCollection<string> CreateObservableCollection(IEnumerable collection)
        {
            var observableCollection = new ObservableCollection<string>();

            foreach (var item in collection)
            {
                observableCollection.Add(item.ToString());
            }

            return observableCollection;
        }
        public static IEnumerable<string> BuildLinksCollection(string filePath)
        {
            try
            {
                return File.ReadLines(filePath);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
