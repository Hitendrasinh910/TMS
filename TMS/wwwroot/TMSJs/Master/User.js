// ======================================================
// CONFIG
// ======================================================
const API = "/api/master/user";
let entryModal = null;

// ======================================================
// DOM CACHE
// ======================================================
const DOM = {
    id: () => document.getElementById("hdnId"),
    userType: () => document.getElementById("ddlUserType"), // Maps to UserType
    userName: () => document.getElementById("txtUserName"),
    password: () => document.getElementById("txtPassword"),
    fullName: () => document.getElementById("txtFullName"),
    email: () => document.getElementById("txtEmail"),
    contactNo: () => document.getElementById("txtContactNo"), // Maps to ContactNo
    save: () => document.getElementById("btnSave"),
    tbody: () => document.getElementById("tblBody"),
    modal: () => document.getElementById("addModal")
};

// ======================================================
// INIT
// ======================================================
document.addEventListener("DOMContentLoaded", async () => {

    const btnCreate = document.getElementById("btnCreateNewUser");

    if (btnCreate) {
        const hasAddRight = hasUserRight("User", "Add");
        btnCreate.classList.toggle("disabled", !hasAddRight);
        //if (!hasAddRight) btnCreate.removeAttribute("href");
    }

    await loadDropdown();
    await bindTable();

    $('#userList').DataTable({
        lengthChange: true,
        searching: true,
        pageLength: 10,
        ordering: false,
        language: {
            paginate: {
                next: '<i class="fa fa-angle-double-right"></i>',
                previous: '<i class="fa fa-angle-double-left"></i>'
            }
        }
    });

    entryModal = new bootstrap.Modal(DOM.modal(), { backdrop: "static" });

    DOM.modal().addEventListener("shown.bs.modal", () => {
        setTimeout(() => DOM.userName().focus(), 50);
    });

    DOM.modal().addEventListener("hidden.bs.modal", clearForm);
    DOM.save().addEventListener("click", saveData);

    const pwd = DOM.password();
    const btn = document.getElementById("btnTogglePassword");

    if (pwd && btn) {
        btn.addEventListener("click", () => {
            const isHidden = pwd.type === "password";
            pwd.type = isHidden ? "text" : "password";
            btn.innerHTML = isHidden ? '<i class="fa fa-eye-slash"></i>' : '<i class="fa fa-eye"></i>';
        });
    }
});

// LOAD DROPDOWNS
// ======================================================
async function loadDropdown() {
    try {
        const res = await apiFetch(`${API}/get-all-user-type`);
        const json = await res.json();

        if (json.success && json.data) {
            let options = '<option value="">-- Select User Type --</option>';
            json.data.forEach(p => {
                options += `<option value="${escapeHtml(p.userType)}">${escapeHtml(p.userType)}</option>`;
            });

            // Populate all 3 dropdowns with the exact same party data
            DOM.userType().innerHTML = options;

            // Refresh bootstrap select
            $('#ddlUserType').selectpicker('refresh');
        }
    } catch (err) {
        console.error("Error loading dropdown:", err);
    }
}

// ======================================================
// BIND TABLE
// ======================================================
async function bindTable() {
    try {
        const res = await apiFetch(`${API}/get-all`);
        const json = await res.json();

        if (!json.success) {
            showToast("danger", json.message, "User Master");
            return;
        }

        const tbody = DOM.tbody();
        tbody.innerHTML = "";


        const canEdit = hasUserRight("User", "Update");
        const canDelete = hasUserRight("User", "Delete");

        json.data.forEach(d => {
            const tr = document.createElement("tr");
            tr.id = `row-${d.idUser}`;

            tr.innerHTML = `
                <td><h6 class="mb-0 fs-14 font-w600">${escapeHtml(d.userName)}</h6></td>
                <td>${escapeHtml(d.fullName || "-")}</td>
                <td><span class="badge bg-info">${escapeHtml(d.userType || "USER")}</span></td>
                <td>${escapeHtml(d.contactNo || "-")}</td>
                <td class="text-center">
                    <div class="d-flex justify-content-center">
                    ${canEdit
                    ? `<button onclick="editEntry(${d.idUser})" class="btn btn-primary shadow btn-xs sharp me-1"><i class="fa fa-pencil"></i></button>`
                        : `<button class="btn btn-primary shadow btn-xs sharp me-1 opacity-50" disabled><i class="fa fa-pencil"></i></button>`
                    }

                    ${canDelete
                    ? `<button onclick="deleteEntry(${d.idUser})" class="btn btn-danger shadow btn-xs sharp"><i class="fa fa-trash"></i></button>`
                        : `<button class="btn btn-danger shadow btn-xs sharp opacity-50" disabled><i class="fa fa-trash"></i></button>`
                     }
                 </div>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        console.error(err);

        showToast("danger", "Failed to load data", "User Master");
    }
}

// ======================================================
// EDIT
// ======================================================
async function editEntry(id) {
    try {
        const res = await apiFetch(`${API}/get-by-id/${id}`);
        const json = await res.json();

        if (!json.success) throw new Error(json.message);

        const d = json.data;

        DOM.id().value = d.idUser;
        DOM.userName().value = d.userName || "";
        DOM.password().value = d.password || "";
        DOM.fullName().value = d.fullName || "";
        DOM.userType().value = d.userType || "";
        DOM.email().value = d.email || "";
        DOM.contactNo().value = d.contactNo || "";

        $('#ddlUserType').selectpicker('refresh');
        entryModal.show();

    } catch (err) {
        showToast("danger", err.message, "User Master");
    }
}

// ======================================================
// SAVE
// ======================================================
async function saveData() {
    if (!DOM.userName().value.trim() || !DOM.fullName().value.trim()) {
        showToast("danger", "Required fields missing", "User Master");
        return;
    }

    const isUpdate = Number(DOM.id().value || 0) > 0;

    // Matches your C# MasterUser class exactly
    const dto = {
        idUser: Number(DOM.id().value || 0),
        userType: DOM.userType().value,
        userName: DOM.userName().value.trim(),
        password: DOM.password().value.trim(),
        fullName: DOM.fullName().value.trim(),
        email: DOM.email().value.trim(),
        contactNo: DOM.contactNo().value.trim()
    };

    DOM.save().disabled = true;

    try {
        const res = await apiFetch(`${API}/save`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        const json = await res.json();
        if (!json.success) throw new Error(json.message);

        showToast("success", json.message, "User Master");
        bindTable();

        if (isUpdate) {
            entryModal.hide();
        } else {
            clearForm();
            setTimeout(() => DOM.userName().focus(), 100);
        }
    } catch (err) {
        showToast("danger", err.message, "User Master");
    } finally {
        DOM.save().disabled = false;
    }
}

// ======================================================
// CLEAR
// ======================================================
function clearForm() {
    DOM.id().value = 0;
    DOM.userName().value = "";
    DOM.password().value = "";
    DOM.fullName().value = "";
    DOM.email().value = "";
    DOM.contactNo().value = "";
    DOM.userType().value = "";

    DOM.password().type = "password";
    const eyeBtn = document.getElementById("btnTogglePassword");
    if (eyeBtn) eyeBtn.innerHTML = '<i class="fa fa-eye"></i>';

    $('#ddlUserType').selectpicker('refresh');
}

// ======================================================
// AUTO PASSWORD GENERATOR
// ======================================================
function generatePasswordWithName(name) {
    const clean = (name || "").replace(/[^a-zA-Z]/g, "").trim();
    const namePart = clean.length >= 4
        ? clean.substring(0, 2).toUpperCase() + clean.substring(2, 4).toLowerCase()
        : (clean.substring(0, 2).toUpperCase() + "Us");

    const numbers = Math.floor(100 + Math.random() * 900);
    const special = "@#$%"[Math.floor(Math.random() * 4)];

    return `${namePart}${numbers}${special}`;
}

DOM.fullName().addEventListener("blur", () => {
    if (Number(DOM.id().value || 0) === 0) {
        DOM.password().value = generatePasswordWithName(DOM.fullName().value);
    }
});

document.getElementById("btnGeneratePassword").addEventListener("click", function () {
    const pwdInput = DOM.password();
    pwdInput.value = generatePasswordWithName(DOM.fullName().value);
    pwdInput.type = "text";

    const eyeBtn = document.getElementById("btnTogglePassword");
    if (eyeBtn) eyeBtn.innerHTML = '<i class="fa fa-eye-slash"></i>';
});