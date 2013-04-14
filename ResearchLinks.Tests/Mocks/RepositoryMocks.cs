﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResearchLinks.Data.Models;
using ResearchLinks.Data.Repository;
using Moq;

namespace ResearchLinks.Tests.Mocks
{
    public enum ReturnType { Exception, Normal};
    public class RepositoryMocks
    {
        private List<Project> projects = new List<Project>();

        public RepositoryMocks() {
            // projects
            projects.Add(new Project() { Name = "Test Project 1", UserName = "james", ProjectId = 1 });
            projects.Add(new Project() { Name = "Test Project 2", UserName = "james", ProjectId = 2 });
            projects.Add(new Project() { Name = "Test Project 3", UserName = "john", ProjectId = 3 });
        
        }

        public Mock<IProjectRepository> GetProjectsRepository(ReturnType returnType)
        {
            var mockProjectRepository = new Mock<IProjectRepository>();

            var projectListJames = new List<Project>();
            projectListJames.Add(projects[0]);
            projectListJames.Add(projects[1]);

            var projectListJohn = new List<Project>();
            projectListJohn.Add(projects[2]);

            if (returnType == ReturnType.Exception) {
                mockProjectRepository.Setup(m => m.GetByUser(It.IsAny<string>())).Throws(new ApplicationException("Database exception!"));
                mockProjectRepository.Setup(m => m.GetByUser(It.IsAny<int>(), It.IsAny<string>())).Throws(new ApplicationException("Database exception!"));
                mockProjectRepository.Setup(m => m.Insert(It.IsAny<Project>())).Throws(new ApplicationException("Database exception!"));
                mockProjectRepository.Setup(m => m.Delete(It.IsAny<Project>())).Throws(new ApplicationException("Database exception!"));
                mockProjectRepository.Setup(m => m.Commit()).Throws(new ApplicationException("Database exception!"));
            } else {
                mockProjectRepository.Setup(m => m.GetByUser("james")).Returns(projectListJames.AsQueryable());
                mockProjectRepository.Setup(m => m.GetByUser("john")).Returns(projectListJohn.AsQueryable());
                mockProjectRepository.Setup(m => m.GetByUser(1,"james")).Returns(projects[0]);
                mockProjectRepository.Setup(m => m.GetByUser(1, "john")).Returns((Project)null);
                mockProjectRepository.Setup(m => m.Delete(It.IsAny<Project>()));
                mockProjectRepository.Setup(m => m.Insert(It.IsAny<Project>()));
                mockProjectRepository.Setup(m => m.Commit());
            }

            return mockProjectRepository;
        }
    }
}
