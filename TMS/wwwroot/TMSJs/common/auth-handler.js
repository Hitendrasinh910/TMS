// -----------------------------------
// PAGE RIGHTS CHECK
// -----------------------------------
//window.guardPageAccess = function (formName) {
//    if (!formName) return true;

//    if (typeof hasUserRight !== "function") {
//        console.warn("hasUserRight() is not available.");
//        return true;
//    }

//    const allowed = hasUserRight(formName, "View");

//    if (!allowed) {
//        Swal.fire({
//            icon: "error",
//            title: "Access Denied",
//            text: "You don't have permission to view this page.",
//            confirmButtonText: "OK"
//        }).then(() => {
//            window.location.href = "/Home/Index";
//        });

//        return false;
//    }

//    return true;
//};

// -----------------------------------
// COMMON API FETCH
// -----------------------------------
async function apiFetch(url, options = {}) {
    const token = localStorage.getItem("auth_token") || sessionStorage.getItem("auth_token");

    options.headers = {
        "Content-Type": "application/json",
        ...(token ? { "Authorization": `Bearer ${token}` } : {}),
        ...options.headers
    };

    const response = await fetch(url, options);

    if (response.status === 401) {
        localStorage.removeItem("auth_token");
        sessionStorage.removeItem("auth_token");
        localStorage.removeItem("jwtToken");
        sessionStorage.removeItem("jwtToken");

        window.location.href = "/Account/Login";
        return null;
    }

    return response;
}

// -----------------------------------
// LOGOUT
// -----------------------------------
function logoutUser() {
    Swal.fire({
        icon: "warning",
        title: "Logout?",
        text: "Are you sure you want to sign out?",
        showCancelButton: true,
        confirmButtonText: "Yes, Logout",
        cancelButtonText: "Cancel"
    }).then(result => {
        if (!result.isConfirmed) return;

        localStorage.removeItem("auth_token");
        sessionStorage.removeItem("auth_token");

        localStorage.removeItem("jwtToken");
        sessionStorage.removeItem("jwtToken");

        localStorage.removeItem("UserData");
        localStorage.removeItem("Username");

        localStorage.clear();
        sessionStorage.clear();

        document.cookie = "jwtToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        document.cookie = "auth_token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";

        showToast("info", "Logged out successfully!");

        setTimeout(() => {
            window.location.href = "/Account/Login";
        }, 700);
    });
}