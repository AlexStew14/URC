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
 *    Contains JavaScript code to upload a resume file.
 */

async function AJAXSubmitResume(oFormElement) {
    const formData = new FormData(oFormElement);
    try {
        $("#spinner").show();
        const response = await fetch(oFormElement.action, {
            method: 'POST',
            body: formData
        });

        data = await response.json();

        if (response.ok) {
            $("#resume").val(`${data.message}`);
        }
        else {
            $("#response").html("Error: " + data.message);
        }
    }
    catch (error) {
        $("#response").html("Error: " + error);
        console.log('Error:', error);
        console.log(response);
    }
    finally {
        $("#spinner").hide();
    }
}