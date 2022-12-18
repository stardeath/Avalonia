using System.IO;
using MicroCom.CodeGenerator;
using Nuke.Common;

partial class Build : NukeBuild
{
    Target GenerateCppHeaders => _ => _.Executes(() =>
    {

    });
}