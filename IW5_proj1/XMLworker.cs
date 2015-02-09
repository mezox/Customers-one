using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.IO;
using System;
using System.Text;  //encoding


namespace IW5_proj1
{
    public class XMLWorker<T> : IWorker<T> where T : new()
    {
        #region Constructors
        public XMLWorker()
        {
            _filePath = @"Customers.xml";
        }

        public XMLWorker(string path)
        {
            _filePath = path;
        }
        #endregion

        public IWorker<T> IWorker
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Sets the output file for XML worker, used when clicked on save as
        /// menu item.
        /// </summary>
        /// <param name="newFile">The new file.</param>
        public void setOutputFile(string newFile)
        {
            this._filePath = newFile;
        }

        public T Load(out string exception)
        {
            exception = "";
            var s = new XmlSerializer(typeof(T));

            var loaded = default(T);

            try
            {
                using (var xmlReader = new XmlTextReader(this._filePath))
                {
                    loaded = (T)s.Deserialize(xmlReader);
                }
            }
            catch (XmlException ex)
            {
                if (ex.Message.Contains("Root element is missing"))
                {
                    exception = "XML file is empty!";
                }
            }
            catch (FileNotFoundException ex)
            {
                exception = "File not found!";
                Debug.WriteLine(exception + " Exception: {0}", ex);
            }
            catch (InvalidOperationException ex)
            {
                exception = "Object cannot be deserialized!";
                Debug.WriteLine(exception + " Exception: {0}", ex);
            }
            catch (Exception ex)
            {
                exception = "XML file load failed!";
                Debug.WriteLine( exception + "General exception: {0}", ex);
            }
            finally
            {
                if (loaded == null)
                {
                    loaded = new T();
                }
            }
            return loaded;
        }

        public void Save(T group)
        {
            var s = new XmlSerializer(typeof(T));

            try
            {
                using (var xmlWriter = new XmlTextWriter(_filePath, Encoding.UTF8))
                {
                    s.Serialize(xmlWriter, group);
                }
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine("Object cannot be serialized! Exception: {0}", ex);
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine("File not found! Exception: {0}", ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("General exception: {0}", ex);
            }
        }


        private string _filePath;
    }
}
