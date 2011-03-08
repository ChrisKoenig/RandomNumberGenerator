using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Messaging;

namespace RandomNumberGenerator.ViewModels
{
    public class ViewModelLocator
    {
        private const string STR_MainViewModel = "MainViewModel";

        public ViewModelLocator()
        {
            CreateMainViewModel();
        }

        public void RestoreState()
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(STR_MainViewModel))
                {
                    using (var stream = store.OpenFile(STR_MainViewModel, FileMode.Open))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var data = reader.ReadToEnd();
                            var serializer = new XmlSerializer(typeof(MainViewModel));
                            _main = (MainViewModel)serializer.Deserialize(new StringReader(data));
                        }
                    }
                }
            }
        }

        public void SaveState()
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(STR_MainViewModel))
                    store.DeleteFile(STR_MainViewModel);
                using (var fs = store.CreateFile(STR_MainViewModel))
                {
                    var serializer = new XmlSerializer(typeof(MainViewModel));
                    serializer.Serialize(fs, MainViewModel);
                }
            }
        }

        #region MainViewModel

        private static MainViewModel _main;

        /// <summary>
        /// Gets the MainViewModel property.
        /// </summary>
        public static MainViewModel MainViewModelStatic
        {
            get
            {
                if (_main == null)
                {
                    CreateMainViewModel();
                }

                return _main;
            }
        }

        /// <summary>
        /// Gets the MainViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel MainViewModel
        {
            get
            {
                return MainViewModelStatic;
            }
        }

        /// <summary>
        /// Provides a deterministic way to delete the MainViewModel property.
        /// </summary>
        public static void ClearMainViewModel()
        {
            _main.Cleanup();
            _main = null;
        }

        /// <summary>
        /// Provides a deterministic way to create the MainViewModel property.
        /// </summary>
        public static void CreateMainViewModel()
        {
            if (_main == null)
            {
                _main = new MainViewModel();
            }
        }

        #endregion MainViewModel

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            ClearMainViewModel();
        }
    }
}