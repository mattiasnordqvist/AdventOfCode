using System.Threading.Tasks.Dataflow;

namespace App
{
    public class AsyncIntCodeComputer : IntCodeComputer
    {
        public BufferBlock<long> Input { get; } = new BufferBlock<long>();
        public BufferBlock<long> Output { get; } = new BufferBlock<long>();
        public AsyncIntCodeComputer()
        {
            ReadInput = () => Input.Receive();
            WriteOutput = (o) => Output.Post(o);
        }


    }
}