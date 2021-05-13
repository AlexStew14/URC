/**
 * Author:    Jantzen Allphin
 * Partner:   None
 * Date:      October 3, 2020
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
 *    Contains JavaScript code to interface with the User/Role Admin page. This code specifically implements 
 *    the ability to change the roles of users.
 */

$(function () {
    $(document).on('click', '.octicon-dash-red', function () {
        swal({
            title: "Add role?",
            text: "Once confirmed, this role will be added to this user.",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willAdd) => {
                if (willAdd) {
                    swal({
                        title: "Adding...",
                        buttons: false
                    });
                    $.ajax({
                        url: '/Admin/Change_Role',
                        type: 'POST',
                        context: $(this),
                        data: { user_id: $(this).parent().attr('id'), role: $(this).attr('id'), add_remove: 'add' }
                    })
                        .done(function (result) {
                            if ($(this).attr('id') == "Administrator")
                                $(this).replaceWith(`<td id="Administrator" class="octicon-check-green dt-center"><i class="octicon octicon-check"></i></td>`);
                            else if ($(this).attr('id') == "Professor")
                                $(this).replaceWith(`<td id="Professor" class="octicon-check-green dt-center"><i class="octicon octicon-check"></i></td>`);
                            else
                                $(this).replaceWith(`<td id="Student" class="octicon-check-green dt-center"><i class="octicon octicon-check"></i></td>`);
                            swal({
                                title: "Role added!",
                                icon: "success",
                            });
                        })
                        .fail(function (result) {
                            swal({
                                title: "Unexpected error occurred!",
                                text: "Please try again later.",
                                icon: "error"
                            });
                        })
                        .always(function () {
                            console.log("Attempted role change completed.")
                        })
                }
            });
    })

    $(document).on('click', '.octicon-check-green', function () {
        swal({
            title: "Remove role?",
            text: "Once confirmed, this role will be removed from this user.",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willAdd) => {
                if (willAdd) {
                    swal({
                        title: "Removing...",
                        buttons: false
                    });
                    $.ajax({
                        url: '/Admin/Change_Role',
                        type: 'POST',
                        context: $(this),
                        data: { user_id: $(this).parent().attr('id'), role: $(this).attr('id'), add_remove: 'remove' }
                    })
                        .done(function (result) {
                            if ($(this).attr('id') == "Administrator")
                                $(this).replaceWith(`<td id="Administrator" class="octicon-dash-red dt-center"><i class="octicon octicon-dash"></i></td>`);
                            else if ($(this).attr('id') == "Professor")
                                $(this).replaceWith(`<td id="Professor" class="octicon-dash-red dt-center"><i class="octicon octicon-dash"></i></td>`);
                            else
                                $(this).replaceWith(`<td id="Student" class="octicon-dash-red dt-center"><i class="octicon octicon-dash"></i></td>`);
                            swal({
                                title: "Role removed!",
                                icon: "success",
                            });
                        })
                        .fail(function (result) {
                            swal({
                                title: "Unexpected error occurred!",
                                text: "Please try again later.",
                                icon: "error"
                            });
                        })
                        .always(function () {
                            console.log("Attempted role change completed.")
                        })
                }
            });
    })
})