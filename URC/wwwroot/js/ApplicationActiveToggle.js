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
 *    Contains JavaScript code to set a student application to active/inactive. 
 */

// get antiforgery token
var validation_token = $(`input[name="__RequestVerificationToken"]`).val();
var headers = {};
headers['RequestVerificationToken'] = validation_token;

$(function () {
    $(document).on('click', '#btn-toggle-active', function () {
        swal({
            title: "Set Application Inactive?",
            text: "Once confirmed, this application will be inactive.",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((result) => {
                if (result) {
                    swal({
                        title: "Setting Inactive...",
                        buttons: false
                    });
                    $.ajax({
                        url: '/Students/Apply',
                        type: 'POST',
                        headers:headers,
                        context: $(this),
                        data: { id: $(this).parent().attr('id'), toggle: 'remove' }
                    })
                        .done(function (result) {
                            $(this).replaceWith(`<button id="btn-toggle-inactive" class="btn-sm btn-success">Set Application Active</button>`);
                            $("#active-check-box input").prop('checked', false);
                            swal({
                                title: "Application Inactive!",
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
                            console.log("Attempted application active toggle completed.")
                        })
                }
            });
    })

    $(document).on('click', '#btn-toggle-inactive', function () {
        swal({
            title: "Set Application Active?",
            text: "Once confirmed, this application will become active.",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willAdd) => {
                if (willAdd) {
                    swal({
                        title: "Setting Active...",
                        buttons: false
                    });
                    $.ajax({
                        url: '/Students/Apply',
                        type: 'POST',
                        headers:headers,
                        context: $(this),
                        data: { id: $(this).parent().attr('id'), toggle: 'apply' }
                    })
                        .done(function (result) {
                            $(this).replaceWith(`<button id="btn-toggle-active" class="btn-sm btn-danger">Set Application Inactive</button>`);
                            $("#active-check-box input").prop('checked', true);
                            swal({
                                title: "Application Active!",
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
                            console.log("Attempted application active toggle completed.")
                        })
                }
            });
    })
})