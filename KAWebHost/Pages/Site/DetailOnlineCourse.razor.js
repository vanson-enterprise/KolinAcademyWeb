$(document).ready(function () {
    debugger
    $('.web-video').bind('contextmenu', function () { return false; });

    const $video = document.querySelector(".web-video");

    const onTimeUpdate = event => {
        console.log(checkSkipped(event.target.currentTime));
    }

    let prevTime = 0;
    const checkSkipped = currentTime => {
        const skip = [];
        // only record when user skip more than 2 seconds
        const skipThreshold = 2;

        // user skipped part of the video
        if (currentTime - prevTime > skipThreshold) {
            skip.push({
                periodSkipped: currentTime - prevTime,
                startAt: prevTime,
                endAt: currentTime,
            });
            prevTime = currentTime;
            return skip;
        }

        prevTime = currentTime;
        return false;
    }

    $video.addEventListener("play", e => console.log('play'));
    $video.addEventListener("playing", e => console.log('playing'));

    $video.addEventListener("timeupdate", onTimeUpdate);

    $video.addEventListener("ended", e => console.log('ended'));
    $video.addEventListener("pause", e => console.log('pause'));
});