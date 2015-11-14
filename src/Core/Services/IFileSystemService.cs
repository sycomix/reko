﻿#region License
/* 
 * Copyright (C) 1999-2015 John Källén.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Reko.Core.Services
{
    /// <summary>
    /// Abstracts away file system accesses.
    /// </summary>
    public interface IFileSystemService
    {
        Stream CreateFileStream(string filename, FileMode mode);
        Stream CreateFileStream(string filename, FileMode mode, FileAccess access);
        Stream CreateFileStream(string filename, FileMode mode, FileAccess access, FileShare share);
        string MakeRelativePath(string fromPath, string toPath);
    }

    public class FileSystemServiceImpl : IFileSystemService
    {
        public Stream CreateFileStream(string filename, FileMode mode)
        {
            return new FileStream(filename, mode);
        }

        public Stream CreateFileStream(string filename, FileMode mode, FileAccess access)
        {
            return new FileStream(filename, mode, access);
        }

        public Stream CreateFileStream(string filename, FileMode mode, FileAccess access, FileShare share)
        {
            return new FileStream(filename, mode, access, share);
        }

        public string MakeRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (string.Compare(toUri.Scheme, "file", true) == 0)
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }
            return relativePath;
        }

    }
}
