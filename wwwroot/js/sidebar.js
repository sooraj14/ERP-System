

    $(document).ready(function () {

        $("#studentSelect").click(function (e) {
            e.preventDefault();
            var selectedValue = $(this).val();

            if (selectedValue === "add-students") {
                $("#form-container-student").show();
                $("#form-container-teacher").hide();
                $("#view-students-container").hide();
                $("#studentsinthisbranch").hide();
                $("#viewteachersstream").hide();
                $("#teachersinthisbranch").hide();
            }
            else if (selectedValue === "view-students") {
                console.log(selectedValue)
                $("#view-students-container").show();

                $(".selectbranchforstudents").show();
                $("#viewteachersstream").hide();
                $("#form-container-student").hide();
                $("#form-container-teacher").hide();
                $("#studentsinthisbranch").hide();
                $("#teachersinthisbranch").hide();
            } else {
                $("#form-container-student").hide();
                $("#view-students-container").hide();
                $("#form-container-teacher").hide();
            }
        });


    $("#teacherselect").click(function () {
        if ($(this).val() === "add-teacher") {
            Console.log("addteacher");
            $("#form-container-teacher").show();
           
                $("#form-container-student").hide();
                $("#view-students-container").hide();
                $("#studentsinthisbranch").hide();
                $("#viewteachersstream").hide();
                $("#view-students-container").hide();
                $("#teachersinthisbranch").hide();
                $("#studentsinthisbranch").hide();
                    }
                    else if ($(this).val() === "view-teacher") {
                        console.log("viewteacherisexecuting");
                          $("#viewteachersstream").show();
                        $("#form-container-teacher").hide();
                        $("#form-container-student").hide();
                        $("#view-students-container").hide();
            $("#studentsinthisbranch").hide();
            $("#view-students-container").hide();
                    }
 });


    $("#addbranch").change(function () {
      if ($(this).val() === "add-branch") {
        $("#form-container-branch").show();
    $("#form-container-student").hide();
    $("#form-container-teacher").hide();
    $("#view-students-container").hide();
    $("#studentsinthisbranch").hide();
                    } else {
        $("#form-container-branch").hide();
                    }
                });


    $(".selectbranchforstudents").click(function () {
        let streamId = $(this).data("stream-id");

    $("#studentsinthisbranch").show();

    $(".student-row").each(function () {
        let rowStreamId = $(this).data("stream-id");

    if (rowStreamId == streamId) {
        $(this).show(); 
                        } else {
        $(this).hide();
                        }
                    });
                });

    $(".selectbranchforteacher").click(function () {
        let streamId = $(this).data("stream-id");

    $("#teachersinthisbranch").show();

    $(".teacher-row").each(function () {
        let particularid = $(this).data("stream-id");

    if (particularid == streamId) {
        $(this).show();
                        } else {
        $(this).hide();
                        }

                    });
                });


    $("#streamselect").change(function() {
                 var streamstored = ($(this).val());
    $("#hiddeninput").val(streamstored);
             })
           
            
    });



    $(document).ready(function () {

        $("#branch-select").change(function (e) {
            e.preventDefault();
            const value = $(this).val();
            console.log(value);
            if (value === "add-branch") {
                $("#addbranch").show();
                $("#viewbeanch").hide();
                $("#addsubjectdetails").hide();
                $("#viewsubjectdetails").hide();
                $("#viewsubjectsstream").hide();
            }
            else if (value === "view-branch") {
                $("#viewbeanch").show();
                $("#addbranch").hide();
                $("#addsubjectdetails").hide();
                $("#viewsubjectdetails").hide();
                $("#viewsubjectsstream").hide();
             
            } else {
                $("#addbranch").hide();
                $("#viewbeanch").hide();
                $("#viewsubjectdetails").hide();
                $("#viewsubjectsstream").hide();
            }
        });


    $("#subject-select").change(function () {
            const value = $(this).val();
    if (value === "add-subject") {
        $("#addsubjectdetails").show();
    $("#addbranch").hide();
        $("#viewbeanch").hide();
        $("#viewsubjectsstream").hide();
    $("#viewsubjectdetails").hide();
             
            } else if (value === "view-subject") {
        /*$("#viewsubjectdetails").show();*/
        $("#viewsubjectsstream").show();
    $("#addbranch").hide();
    $("#addsubjectdetails").hide();
    $("#viewbeanch").hide();    
            } else {
        $("#addsubjectdetails").hide();
        $("#viewsubjectdetails").hide();
        $("#viewsubjectsstream").hide();
            }
        });


    $("#addbranchbtn").click(function () {
        $("#textbox-container").append(
            '<input type="text" name="stream_name[]" class="form-control mt-2" placeholder="Enter branch name" />'
        );
        });


    $("#addsubjectbtn").click(function () {
        $("#subjectadded").append('<input type="text" name="subject_name[]" class="form-control mt-2" placeholder="Enter branch name" />')
    });


        $(".selectbranchforsubject").click(function () {
            let streamId = $(this).data("stream-id");

            $("#viewsubjectdetails").show();

            $(".subject-row").each(function () {
                let rowStreamId = $(this).data("stream-id");

                if (rowStreamId == streamId) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            });
        });

    });

$(document).ready(function () {
    $("#branchSelect").on("change", function () {
        var stream_id = $(this).val();


        $("#semesterSelect").on("change", function () {
            var sem_id = $(this).val();

            if (!stream_id) {

                $("#subjectSelect").html('<option value="" disabled selected>Select Teacher</option>');
                return;

            }
            $.ajax({
                type: 'GET',
                url: '/Admin/AccademicStructure?handler=Branchwithsubject',
                data: ({ stream_id: stream_id, sem_id: sem_id }),
                success: function (teachers) {
                    $("#subjectSelect").html('<option value="" disabled selected>Select Subject</option>');

                      if (teachers.length > 0) {
                        $.each(teachers, function (index, teacher) {

                            $("#subjectSelect").append(`<option value="${teacher.sub_id}">${teacher.sub_name}</option>`);


                        });
                    } else {

                        $("#subjectSelect").append('<option value="" disabled>No teachers available</option>');
                    }
                },
                error: function () {
                    alert('Error fetching teachers. Please try again.');
                }
            });

        });
    });
});





$(document).ready(function () {
    $("#subject-select").on("change",function () {
        if($(this).val() === "add-Teacher-subject") {
            $("#Teacher-Faculty-container").show();
            $("#showbranchtogetteacherdata").hide();
        } else if($(this).val() === "view-Teacher-subject") {
            $("#showbranchtogetteacherdata").show();
            $("#Teacher-Faculty-container").hide();
        }
       
    });
});


$(document).ready(function () {
    $(".selectbranchforteachersubject").on("click", function () {
        let value1 = $(this).data("stream-id");

        $("#viewsubjectdetailsofteacher").show();


        $(".facultysubjects").each(function () {
            let value2 = $(this).data("stream-id");


            if (value2 == value1) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});

