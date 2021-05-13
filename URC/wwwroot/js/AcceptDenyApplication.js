/**
 * Author:    Jantzen Allphin
 * Partner:   None
 * Date:      December 6, 2020
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
 *    Contains JavaScript code to accept or deny an application.
 */

$(function () {
    $(document).on('click', '.octicon-x-red', function () {
        swal({
            title: "Deny application?",
            text: "Once confirmed, this application will be denied.",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willAdd) => {
                if (willAdd) {
                    swal({
                        title: "Denying...",
                        buttons: false
                    });
                    $.ajax({
                        url: '/Applications/AcceptDeny',
                        type: 'POST',
                        context: $(this),
                        data: { app_id: $(this).parent().attr('id'), accept_deny: 'deny' }
                    })
                        .done(function (result) {
                            swal({
                                title: "Application denied!",
                                icon: "success",
                            })
                                .then((ok) => {
                                    location.reload();
                                })
                        })
                        .fail(function (result) {
                            swal({
                                title: "Unexpected error occurred!",
                                text: "Please try again later.",
                                icon: "error"
                            });
                        })
                        .always(function () {
                            console.log("Attempted deny application completed.")
                        })
                }
            });
    })

    $(document).on('click', '.octicon-check-green', function () {
        swal({
            title: "Accept application?",
            text: "Once confirmed, this application will be accepted.",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willAdd) => {
                if (willAdd) {
                    swal({
                        title: "Accepting...",
                        buttons: false
                    });
                    $.ajax({
                        url: '/Applications/AcceptDeny',
                        type: 'POST',
                        context: $(this),
                        data: { app_id: $(this).parent().attr('id'), accept_deny: 'accept' }
                    })
                        .done(function (result) {
                            swal({
                                title: "Application accepted!",
                                icon: "success",
                            })
                                .then((ok) => {
                                    location.reload();
                                })
                        })
                        .fail(function (result) {
                            swal({
                                title: "Unexpected error occurred!",
                                text: "Please try again later.",
                                icon: "error"
                            });
                        })
                        .always(function () {
                            console.log("Attempted accept application completed.")
                        })
                }
            });
    })
})