const onlineCoursePageJs = function () {
    this.init = (dotnetRef, funcName) => {

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

        //$video.addEventListener("play", e => console.log('play'));
        //$video.addEventListener("playing", e => console.log('playing'));
        //$video.addEventListener("timeupdate", onTimeUpdate);
        //$video.addEventListener("pause", e => console.log('pause'));

        $video.addEventListener("ended", e => {
            dotnetRef.invokeMethodAsync(funcName);
            console.log('ended')
        });
    }

    this.loadVideo = () => {
        setTimeout(() => {
            document.getElementById("main-video").load();
        },500)
    }
}
window.onlineCoursePageJs = new onlineCoursePageJs();
