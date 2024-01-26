document.addEventListener('DOMContentLoaded', function () {
    const audio = document.getElementById('audio');
    const playPauseBtn = document.querySelector('.play-pause');
    const progressBar = document.querySelector('.progress-bar');
    const progress = document.querySelector('.progress');
    const timeDisplay = document.querySelector('.time-display');
    const repeatBtn = document.querySelector('.repeat');
    const volumeIcon = document.querySelector('.volume-icon');
    const volumeControlContainer = document.querySelector('.volume-control-container');
    const volumeControl = document.querySelector('.volume-control');

    playPauseBtn.addEventListener('click', togglePlayPause);
    audio.addEventListener('timeupdate', updateProgressBar);
    progressBar.addEventListener('click', handleProgressBarClick);
    repeatBtn.addEventListener('click', toggleRepeat);
    volumeIcon.addEventListener('mouseover', showVolumeControl);
    volumeIcon.addEventListener('mouseout', hideVolumeControl);
    volumeControl.addEventListener('input', adjustVolume);
    progress.style.width = 0;

    function togglePlayPause() {
        if (audio.paused) {
            audio.play();
            playPauseBtn.textContent = '❚❚';
        } else {
            audio.pause();
            playPauseBtn.textContent = '►';
        }
    }

    function updateProgressBar() {
        const currentTime = audio.currentTime;
        const duration = audio.duration;
        const progressPercent = (currentTime / duration) * 100;
        progress.style.width = progressPercent + '%';

        const currentMinutes = Math.floor(currentTime / 60);
        const currentSeconds = Math.floor(currentTime % 60);
        const durationMinutes = Math.floor(duration / 60);
        const durationSeconds = Math.floor(duration % 60);
        timeDisplay.textContent = `${currentMinutes}:${currentSeconds < 10 ? '0' : ''}${currentSeconds} / ${durationMinutes}:${durationSeconds < 10 ? '0' : ''}${durationSeconds}`;
    }

    function handleProgressBarClick(event) {
        const clickX = event.clientX - progressBar.getBoundingClientRect().left;
        const progressBarWidth = progressBar.clientWidth;
        const seekTime = (clickX / progressBarWidth) * audio.duration;
        audio.currentTime = seekTime;
    }

    function toggleRepeat() {
        audio.loop = !audio.loop;
        if (audio.loop) {
            repeatBtn.classList.add('active');
        } else {
            repeatBtn.classList.remove('active');
        }
    }

    function showVolumeControl() {
        volumeControlContainer.style.display = 'flex';
    }

    function hideVolumeControl() {
        volumeControlContainer.style.display = 'none';
    }

    function adjustVolume() {
        const volume = volumeControl.value / 100;
        audio.volume = volume;
    }
});
