namespace Sequin.Pipeline
{
    using System;

    public class MissingIssueCommandStageException : Exception
    {
        internal MissingIssueCommandStageException() : base("The IssueCommand stage is not present in the pipeline")
        {

        }
    }
}
