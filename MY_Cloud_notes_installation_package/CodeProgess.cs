using SevenZip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MY_Cloud_notes_installation_package
{
    class CodeProgress : ICodeProgress
    {
        public delegate void ProgressDelegate(Int64 fileSize, Int64 processSize);

        public ProgressDelegate m_ProgressDelegate = null;

        public CodeProgress(ProgressDelegate del)
        {
            m_ProgressDelegate = del;
        }

        public void SetProgress(Int64 inSize, Int64 outSize)
        {

        }

        public void SetProgressPercent(Int64 fileSize, Int64 processSize)
        {
            m_ProgressDelegate(fileSize, processSize);
        }
    }
}
