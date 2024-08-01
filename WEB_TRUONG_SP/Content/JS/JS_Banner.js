$(document).ready(function () {
    var slideIndex = 0;
    var slides = $(".slide");
    var slideInterval;

    function showSlides() {
        for (var i = 0; i < slides.length; i++) {
            slides[i].style.display = "none";
        }
        slideIndex++;
        if (slideIndex > slides.length) {
            slideIndex = 1;
        }
        slides[slideIndex - 1].style.display = "block";
        resetInterval();
    }

    function resetInterval() {
        clearInterval(slideInterval);
        slideInterval = setInterval(showSlides, 5000); 
    }

    showSlides();

    $(".prev").click(function () {
        slideIndex -= 2;
        if (slideIndex < 0) {
            slideIndex = slides.length - 1;
        }
        showSlides();
    });

    $(".next").click(function () {
        showSlides();
    });

    resetInterval();
});
