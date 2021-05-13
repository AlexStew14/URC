/**
 * Author:    Jantzen Allphin
 * Partner:   None
 * Date:      October 23, 2020
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jantzen Allphin - This work may not be copied for use in Academic Coursework.
 *
 * I, Jantzen Allphin, certify that I wrote this code (below) from scratch and did
 * not copy it in part or whole from another source.  Any references used
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *
 *    Contains JavaScript code to enable searching for students from the Search page.
 */

// get antiforgery token
var validation_token = $(`input[name="__RequestVerificationToken"]`).val();
$.ajaxSetup({
    headers: {
        'RequestVerificationToken': validation_token
    }
});

// register click action
$("#search-button").click(search_students);

/**
 * Searches for the students that matches the search input.
 */
function search_students() {
    var input = $("#search-input").val().toUpperCase();

    var data = {
        message: input
    };

    $.post("/Students/Find", data)
        .done(function (result) {
            // first clear out all previous cards
            $("#cards").empty();

            // add new cards
            var json = JSON.parse(result);

            // check for no results
            if (json.length == 0)
                $("#cards").append(`<h1 style="text-align: center; padding: 10px;" class="gray-background text-light">No Results Found</h1>`);

            for (var i = 0; i < json.length; i++) {
                // name
                var student_name = json[i].StudentName;
                // uid
                var student_uid = json[i].Uid;
                // gpa
                var student_gpa = json[i].GPA;
                // skills
                var student_skills = "";
                for (var j = 0; j < json[i].StudentSkills.length; j++) {
                    if (j == json[i].StudentSkills.length - 1)
                        student_skills += json[i].StudentSkills[j];
                    else
                        student_skills += json[i].StudentSkills[j] + ", ";
                }
                if (student_skills.length >= 500)
                    student_skills = student_skills.substring(0, 500) + " ...";
                // statement
                if (json[i].PersonalStatement.length >= 500)
                    var student_statement = json[i].PersonalStatement.substring(0, 500) + " ...";
                else
                    var student_statement = json[i].PersonalStatement;

                // id
                var student_id = json[i].ID;

                add_card(student_name, student_uid, student_gpa, student_skills, student_statement, student_id);
            }
        })
        .fail(function (result) {
            console.log("oops");
        })
        .always(function (result) {
            console.log("completed displaying search results")
        })

    // don't refresh the page
    return false;
}

/**
 * Adds a new card for the given student.
 */
function add_card(student_name, student_uid, student_gpa, student_skills, student_statement, student_id) {
    var new_card = $("#template").clone();

    $("#cards").append(new_card);
    $(new_card).find("#student-name").html(student_name);
    $(new_card).find("#student-uid").html(`UID: ${student_uid}`);
    $(new_card).find("#student-gpa").html(`GPA: ${student_gpa}`);
    $(new_card).find("#student-skills").html(student_skills);
    $(new_card).find("#student-statement").html(student_statement);
    $(new_card).find("#app-link").attr("href", `/Students/Details/${student_id}`);
    $(new_card).removeAttr("id");

    new_card.show();
}
