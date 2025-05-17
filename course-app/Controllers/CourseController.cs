using basics.Models;
using Microsoft.AspNetCore.Mvc;

namespace basics.Controllers;

public class CourseController : Controller
{

    // IActionResult - calling action method (e.g. index, list)
    // this could also be a string that returns a string value
    // course/index
    // url/course also returns the index page because of the MapControllerRoute (default action setting is set to Index)
    // -------------------------------------------------------------
    // public string Index()
    // {
    //     return "course/index";
    // }
    // -------------------------------------------------------------

    // searchs for Views/Course/Index.cshtml and returns its view
    // also searchs for Views/Shared/Index.cshtml 
    // public IActionResult Index()
    // {
    //     // created a new instance of kurs using model
    //     var kurs = new Course();

    //     kurs.Id = 1;
    //     kurs.Title = "Asp.Net Core";
    //     kurs.Description = "Description";
    //     kurs.Image = "1.jpg";
    //     // to display the kurs object with the view, model must be introduced to the view (cshtml) file
    //     // (@model Course in Views/Course/Index.cshtml)
    //     // this works for only one instance of Course, to view a list of courses, see below (List())
    //     return View(kurs);
    // }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return RedirectToAction("List", "Course");
        }

        var kurs = Repository.GetById(id);
        // to display the kurs object with the view, model must be introduced to the view (cshtml) file
        // (@model Course in Views/Course/Index.cshtml)
        // this works for only one instance of Course, to view a list of courses, see below (List())
        return View(kurs);
    }

    // same as above
    // course/list
    // -----------------------------------------------
    // public string List()
    // {
    //     return "course/list";
    // }
    // -----------------------------------------------

    // searchs for Views/Course/List.cshtml and returns its view
    // also searchs for Views/Shared/List.cshtml 
    // If there are no parameters for View() function, it looks for the cshtml file named same as the method
    // e.g. List() -> View() looks for List.cshtml
    // but List() -> View("CourseList") looks for CourseList.cshtml
    public IActionResult List()
    {
        return View("CourseList", Repository.Courses);
    }
}