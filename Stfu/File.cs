//
//  Stfu — Sam’s Tiny Framework Utilities
//
//  Copyright © 2013–2023 Sam Hocevar <sam@hocevar.net>
//
//  This library is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using System;
using System.IO;
using System.Text;

namespace Stfu
{
    public class AtomicFileWriter : StreamWriter
    {
        public AtomicFileWriter(string path)
          : base($"{path}~")
            => Initialize(path);

        public AtomicFileWriter(string path, bool append)
          : base($"{path}~", append)
            => Initialize(path);

        public AtomicFileWriter(string path, bool append, Encoding encoding)
          : base($"{path}~", append, encoding)
            => Initialize(path);

        public AtomicFileWriter(string path, bool append, Encoding encoding, int bufferSize)
          : base($"{path}~", append, encoding, bufferSize)
            => Initialize(path);

#if false
        public AtomicFileWriter(string path, Encoding encoding, FileStreamOptions options)
          : base($"{path}~", encoding, options)
            => Initialize(path);
#endif

        private void Initialize(string path)
        {
            // Best way to detect path and permission errors early is to try to append
            // to the destination file and immediately close it. Otherwise these issues
            // are not known until the stream is closed.
            using (var _ = new StreamWriter(path, append: true))
            {
            }

            Path = path;
        }

        // Note from the TextWriter.Close() documentation:
        // “In derived classes, do not override the Close method. Instead, override the
        // TextWriter.Dispose(Boolean) method to add code for releasing resources.”
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Note from the TextWriter.Dispose() documentation:
            // ”Dispose(Boolean) can be called multiple times by other objects. When
            // overriding this method, be careful not to reference objects that have
            // been previously disposed of in an earlier call to Dispose.”
            if (disposing && !m_moved)
            {
#if NETCOREAPP || NETSTANDARD
                File.Move(TemporaryPath, Path, overwrite: true);
#else
                File.Move(TemporaryPath, Path);
#endif
                m_moved = true;
            }
        }

        public string Path { get; private set; }

        public string TemporaryPath => $"{Path}~";

        private bool m_moved;
    }
}
