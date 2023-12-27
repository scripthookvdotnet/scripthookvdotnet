//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    public interface IJoaatHashValue
    {
        /// <summary>
        /// Gets the jenkins-one-at-a-time hash.
        /// </summary>
        public uint GetJoaatHash();
    }
}
