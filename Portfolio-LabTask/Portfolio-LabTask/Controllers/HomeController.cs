using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portfolio_LabTask.Models;

namespace Portfolio_LabTask.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Person person = new Person();
            person.name = "Tazrif Yamshit Raim";
            person.githubLink = "https://github.com/Tazrif-Raim";
            person.id = "21-45012-2";
            person.email = "tazrifraim@gmail.com";
            ViewBag.Person = person;
            return View();
        }

        public ActionResult Education()
        {
            List<EducationalBackground> eduList = new List<EducationalBackground>();
            EducationalBackground edu = new EducationalBackground();
            edu.institute = "AIUB";
            edu.degree = "BSc";
            edu.year = "2024";
            eduList.Add(edu);

            edu = new EducationalBackground();
            edu.institute = "ACC";
            edu.degree = "HSC";
            edu.year = "2020";
            eduList.Add(edu);

            edu = new EducationalBackground();
            edu.institute = "ACPS";
            edu.degree = "SSC";
            edu.year = "2018";
            eduList.Add(edu);

            ViewBag.EduList = eduList;
            
            return View();
        }

        public ActionResult Projects()
        {
            List<Project> projectList = new List<Project>();
            
            Project project = new Project();
            project.name = "Webtech";
            project.description = "Webtech Project Description here";
            project.project = "Into The Green";

            projectList.Add(project);

            project = new Project();
            project.name = "CVPR";
            project.description = "CVPR Project Description here";
            project.project = "Defense Model against Adversarial Attack on Neural Network";
            projectList.Add(project);

            project = new Project();
            project.name = "Data Science";
            project.description = "Data Science Project Description here";
            project.project = "Studnets' academic performance prediction ML model";
            projectList.Add(project);

            ViewBag.ProjectList = projectList;

            
            return View();
        }

        public ActionResult Reference()
        {
            return View();
        }

        public ActionResult Personal()
        {       
            return View();
        }
    }
}