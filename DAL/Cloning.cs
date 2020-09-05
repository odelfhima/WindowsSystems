using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace DAL
{
    public static class Cloning
    {
        public static T Copy<T>(this T source)//deep copy
        {
            var isNotSerializable = !typeof(T).IsSerializable;
            if (isNotSerializable)//in case the source is not Serializable
                throw new ArgumentException("The type must be serializable.", "source");
            var sourceIsNull = ReferenceEquals(source, null);
            if (sourceIsNull)//in case that the source is null
                return default(T);
            var formatter = new BinaryFormatter();//binary file
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);//convert the data of the source to a file
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);//enter each of the data in the file to parameter type T.
            }
        }
    }
}
