using BlueDB.Serialize;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlueDB.Data
{
    //public Class12()
    //{
    //    // camada para abstrair a leitura por blocos dos arquivos
    //    // file split by blocks
    //    // blocks with the NTFS block size to max performance

    //    // camada que controla os dados por blocos
    //    // os blocos são de tamanho fixo
    //    // cada bloco representa um dado

    //    // camada que controla os dados de tamanho variavel
    //    // todo dado tem um indice
    //    // os dados podem estar em um ou mais blocos
    //    // os blocos contem uma header


    //    var file = System.IO.File.OpenRead("a");

    //}




    //public class IndexedData<T> : IDisposable, IEnumerable, IEnumerable<T>, IEnumerator, IEnumerator<T> where T: IData
    //{
    //    private FixedDataFile<IData> _index;

    //    public object Current => throw new NotImplementedException();

    //    T IEnumerator<T>.Current => throw new NotImplementedException();

    //    public void Dispose()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IEnumerator GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool MoveNext()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public T Read(uint id)
    //    {
    //        //var indexData = _index.Read(id);
    //        return default(T);
    //    }

    //    public void Reset()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public interface IData
    //{
    //    uint Id { get; set; }
    //}
}