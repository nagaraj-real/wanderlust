
$(document).ready(function() {
    $(".responsive").slick({
        dots: true,
        infinite: false,
        speed: 300,
        slidesToShow: 4,
        slidesToScroll: 4,
        responsive: [
          {
              breakpoint: 1024,
              settings: {
                  slidesToShow: 3,
                  slidesToScroll: 3,
                  infinite: true,
                  dots: true
              }
          },
          {
              breakpoint: 600,
              settings: {
                  slidesToShow: 2,
                  slidesToScroll: 2
              }
          },
          {
              breakpoint: 480,
              settings: {
                  slidesToShow: 1,
                  slidesToScroll: 1
              }
          }
          // You can unslick at a given breakpoint now by adding:
          // settings: "unslick"
          // instead of a settings object
        ]
    });

    dotdotfunction();
    
   

    $("#uploadimage").on('hidden.bs.modal', function () {
        $('#uploader').each(function () {
            this.reset();
        });
          $('div.progress > div.progress-bar').css({ "width": "0%" });
          $('div.progress-bar').html("0%");
          $('.fileformatalert').addClass("hide");
          $('#UploadSubmit').addClass('disabled');
          $('#uploadimage div.statusalert').addClass("hide");
    });

    $("#áddreview").on('hidden.bs.modal', function () {
        $('#SubmitReview').removeClass('disabled');
        $('#reviewtext').val("");
        $('#áddreview div.statusalert').addClass("hide");
        
    });

    $('#reviewrating').barrating({
        theme: 'bootstrap-stars',
        fastClicks: true,
        showSelectedRating: true

});
 
});

function imageslideslinkclick() {
    $('html, body').animate({
        scrollTop: $("#imageslides").offset().top
    }, 800);
    return false;
}

function reviewslideslinkclick() {
    $('html, body').animate({
        scrollTop: $("#reviewslides").offset().top
    }, 800);
    return false;
}

function readmoreclick(labellink) {
    var content = $(labellink).closest(".reviewtext").triggerHandler("originalContent");
    if (content[1] != null)
        $(".modal-body #readmoremodalcontent").append(content[1]);
}


$(document).on('change', '.btn-file :file', function () {
    $('div.progress > div.progress-bar').css({ "width": "0%" });
    $('#uploadimage div.statusalert').addClass("hide");
    $('div.progress-bar').html("0%");
        var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
    input.trigger('fileselect', [numFiles, label]);
    $('.uploadlabeltext').val(label);
    var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) === -1) {
        $('.fileformatalert').removeClass("hide");
        $('#UploadSubmit').addClass('disabled');
    } else {
        $('.fileformatalert').addClass("hide");
        $('#UploadSubmit').removeClass('disabled');
    }
});


function dotdotfunction() {
    $(".reviewdiv .thumbnail .reviewtext").dotdotdot({
        /*	The text to add as ellipsis. */
        ellipsis: '... ',

        /*	How to cut off the text/html: 'word'/'letter'/'children' */
        wrap: 'word',

        /*	Wrap-option fallback to 'letter' for long words */
        fallbackToLetter: true,

        /*	jQuery-selector for the element to keep and put after the ellipsis. */
        after: "a.readmore",

        /*	Whether to update the ellipsis: true/'window' */
        watch: true,

        /*	Optionally set a max-height, if null, the height will be measured. */
        height: null,

        /*	Deviation for the height-option. */
        tolerance: 0,

        /*	Callback function that is fired after the ellipsis is added,
			receives two parameters: isTruncated(boolean), orgContent(string). */
        callback: function (isTruncated, orgContent) {
            if (!isTruncated) {
                $("a", this).remove();
            }
        },

        lastCharacter: {

            /*	Remove these characters from the end of the truncated text. */
            remove: [' ', ',', ';', '.', '!', '?'],

            /*	Don't add an ellipsis if this array contains 
				the last character of the truncated text. */
            noEllipsis: []
        }
    });

  
}



function addrevfailure() {
    $('#áddreview div.statusalert').removeClass("hide");
    $('#áddreview div.statusalert').addClass("alert-danger");
    $('#áddreview div.statusalert').removeClass("alert-success");
    $('#áddreview div.statusalert p').html("Some error occured");
}

function uploadimgfailure() {
    $('#uploadimage div.statusalert').removeClass("hide");
    $('#uploadimage div.statusalert').addClass("alert-danger");
    $('#uploadimage div.statusalert').removeClass("alert-success");
    $('#uploadimage div.statusalert p').html("some error occured");
    $('#UploadSubmit').addClass('disabled');
    $('div.progress > div.progress-bar').css({ "width": "0%" });
    $('div.progress-bar').html("0%");
}