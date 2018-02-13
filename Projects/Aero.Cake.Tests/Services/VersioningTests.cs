﻿using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Testing;
using FluentAssertions;
using NSubstitute;
using Xunit;
using CakeCore = Cake.Core;

namespace Aero.Cake.Services
{
    public class VersioningTests : ServiceFixture<VersionService>
    {
        public VersioningTests()
        {
            ServiceUnderTest = new VersionService(Context, Logger);
        }

        [Fact]
        public void Update_Test()
        {
            //Arrange
            CakeContext.Globber.Match($"{Context.ProjectsPath}/**/*.csproj").Returns(new List<FilePath> {"/p1/p1.csproj"});

            CakeContext.FileSystem.CreateFile("/p1/p1.csproj", ReadFile($"{TestDirectory}/TestFiles/VersionAttributeExists.csproj"));

            //Act
            ServiceUnderTest.UpdateFiles("2.2.2.2", "../projects");

            //Assert
            var content = CakeContext.FileSystem.GetFile("/p1/p1.csproj").Content;
            System.Text.Encoding.ASCII.GetString(content).Should().Contain("<AssemblyVersion>2.2.2.2</AssemblyVersion>");
            System.Text.Encoding.ASCII.GetString(content).Should().Contain("<FileVersion>2.2.2.2</FileVersion>");
            System.Text.Encoding.ASCII.GetString(content).Should().Contain("<Version>2.2.2.2</Version>");
        }

        [Fact]
        public void Update_2_Segment_Test()
        {
            //Arrange
            CakeContext.Globber.Match($"{Context.ProjectsPath}/**/*.csproj").Returns(new List<FilePath> { "/p1/p1.csproj" });

            CakeContext.FileSystem.CreateFile("/p1/p1.csproj", ReadFile($"{TestDirectory}/TestFiles/VersionAttributeExists.csproj"));

            //Act
            ServiceUnderTest.UpdateFiles("171126.1", "../projects");

            //Assert
            var content = CakeContext.FileSystem.GetFile("/p1/p1.csproj").Content;
            System.Text.Encoding.ASCII.GetString(content).Should().Contain("<AssemblyVersion>17.11.26.1</AssemblyVersion>");
            System.Text.Encoding.ASCII.GetString(content).Should().Contain("<FileVersion>17.11.26.1</FileVersion>");
            System.Text.Encoding.ASCII.GetString(content).Should().Contain("<Version>17.11.26.1</Version>");
        }

        [Fact]
        public void Update__When_Attribute_Missing_Exception_Is_Throw()
        {
            //Arrange
            CakeContext.Globber.Match($"{Context.ProjectsPath}/**/*.csproj").Returns(new List<FilePath> { "/p2/p2.csproj" });

            CakeContext.FileSystem.CreateFile("/p2/p2.csproj", ReadFile($"{TestDirectory}/TestFiles/VersionAttributeDoesNotExist.csproj"));

            //Act
            Assert.Throws<CakeCore.CakeException>(() => ServiceUnderTest.UpdateFiles("2.2.2.2", "../projects"));

        }
    }
}