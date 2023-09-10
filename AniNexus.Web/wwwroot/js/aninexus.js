function addClass(element, className) {
    if (!element.classList.contains(className)) {
        element.classList.add(className);
    }
}

function removeClass(element, className) {
    element.classList.remove(className);
}

function toggleClass(element, className) {
    if (element.classList.contains(className)) {
        removeClass(element, className);
    } else {
        addClass(element, className);
    }
}

function toggleTheme() {
    var elem = document.body;
    toggleClass(elem, 'dark');
}

window.setImage = async (imageElementId, imageStream) => {
    const arrayBuffer = await imageStream.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const image = document.getElementById(imageElementId);
    image.onload = () => {
        URL.revokeObjectURL(url);
    }
    image.src = url;
}


document.addEventListener("DOMContentLoaded", () => {
    // Navbar scroll effect
    window.addEventListener("scroll", () => {
        const nav = document.getElementById("main-nav");
        const className = "has-shadow";
        if (window.scrollY > nav.offsetHeight / 2) {
            addClass(nav, className);
        }
        else {
            removeClass(nav, className);
        }
    });
    // Alternating hero text
    (function () {
        const
            words = ['Anime', 'Manga'],
            len = words.length,
            skip_delay = 25,
            speed = 90;
        var
            part,
            i = 0,
            offset = 0,
            forwards = true,
            skip_count = 0;
        setInterval(function () {
            if (forwards) {
                if (offset >= words[i].length) {
                    ++skip_count;
                    if (skip_count == skip_delay) {
                        forwards = false;
                        skip_count = 0;
                    }
                }
            }
            else {
                if (offset == 0) {
                    forwards = true;
                    i++;
                    offset = 0;
                    if (i >= len) {
                        i = 0;
                    }
                }
            }
            part = words[i].substring(0, offset);
            if (skip_count == 0) {
                if (forwards) {
                    offset++;
                }
                else {
                    offset--;
                }
            }
            document.getElementById("hero-word").textContent = part;
        }, speed);
    })();
});