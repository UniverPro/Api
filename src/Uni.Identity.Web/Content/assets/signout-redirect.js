window.addEventListener("load",
    function() {
        // ReSharper disable once VariableCanBeMadeConst
        var a = document.querySelector("#post-logout-redirect-uri");
        if (a) {
            window.location = a.href;
        }
    });