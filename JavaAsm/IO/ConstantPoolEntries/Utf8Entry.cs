using System;
using System.IO;
using BinaryEncoding;
using JavaAsm.Helpers;

namespace JavaAsm.IO.ConstantPoolEntries {
    internal class Utf8Entry : Entry {
        public string Value { get; }

        public Utf8Entry(string @string) {
            this.Value = @string ?? throw new ArgumentNullException(nameof(@string));
        }

        public Utf8Entry(Stream stream) {
            byte[] data = new byte[Binary.BigEndian.ReadUInt16(stream)];
            stream.Read(data, 0, data.Length);
            this.Value = ModifiedUtf8Helper.Decode(data);
        }

        public override EntryTag Tag => EntryTag.Utf8;

        public override void ProcessFromConstantPool(ConstantPool constantPool) { }

        public override void Write(Stream stream) {
            Binary.BigEndian.Write(stream, ModifiedUtf8Helper.GetBytesCount(this.Value));
            stream.Write(ModifiedUtf8Helper.Encode(this.Value));
        }

        public override void PutToConstantPool(ConstantPool constantPool) { }

        private bool Equals(Utf8Entry other) {
            return this.Value == other.Value;
        }

        public override bool Equals(object obj) {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == this.GetType() && this.Equals((Utf8Entry) obj);
        }

        public override int GetHashCode() {
            return this.Value.GetHashCode();
        }
    }
}