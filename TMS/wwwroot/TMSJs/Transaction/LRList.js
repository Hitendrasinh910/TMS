// ======================================================
// CONFIG
// ======================================================
const API = "/api/transaction/lr";

// ======================================================
// DOM CACHE
// ======================================================
const DOM = {
    tbody: () => document.getElementById("tblBody"),
    table: () => document.getElementById("lrListTable")
};

// ======================================================
// INIT
// ======================================================
document.addEventListener("DOMContentLoaded", async () => {
    // 1. Check permissions and disable the Add button if necessary
    const btnCreate = document.getElementById("btnCreateNewLR");
    if (btnCreate && !hasUserRight("LR", "Add")) {
        // Add Bootstrap's disabled class
        btnCreate.classList.add("disabled");
        // Remove the link so it can't be clicked or opened in a new tab
        btnCreate.removeAttribute("href"); 
        
        // Optional: Change visually so it looks greyed out
        //btnCreate.classList.replace("btn-primary", "btn-secondary"); 
    }
    await bindTable();
});

// ======================================================
// BIND TABLE
// ======================================================
async function bindTable() {
    try {
        const res = await apiFetch(`${API}/get-all`);
        const json = await res.json();

        if (!json.success) {
            showToast("danger", json.message, "LR Register");
            return;
        }

        if ($.fn.DataTable.isDataTable(DOM.table())) {
            $(DOM.table()).DataTable().destroy();
        }

        // 1. Get Rights
        const canEdit = hasUserRight("LR", "Update");
        const canDelete = hasUserRight("LR", "Delete");

        DOM.tbody().innerHTML = "";

        json.data.forEach(d => {
            const tr = document.createElement("tr");

            // Format dates
            const lrDate = d.lrDate ? new Date(d.lrDate).toLocaleDateString('en-GB') : '-';
            const totalAmt = d.totalAmt ? parseFloat(d.totalAmt).toFixed(2) : "0.00";

            // Map the joined dynamic columns from our SQL SP
            const consignor = d.consignorName || 'Unknown';
            const consignee = d.consigneeName || 'Unknown';

            tr.innerHTML = `
                <td class="fw-bold text-primary">${escapeHtml(d.lrNo || '-')}</td>
                <td>${lrDate}</td>
                <td>${escapeHtml(consignor)}</td>
                <td>${escapeHtml(consignee)}</td>
                <td class="fw-bold">₹ ${totalAmt}</td>
                <td>${escapeHtml(d.e_By || '-')}</td>
                <td class="text-center">
                 <div class="d-flex">
                    ${canEdit
                    ? `<a href="/Transaction/LREntry?id=${d.idlr}" class="btn btn-primary shadow btn-xs sharp me-1"><i class="fa fa-pencil"></i></a>`
                    : `<button class="btn btn-primary shadow btn-xs sharp me-1 opacity-50" disabled><i class="fa fa-pencil"></i></button>`
                    }

                    ${canDelete
                            ? `<button onclick="deleteLR(${d.idlr})" class="btn btn-danger shadow btn-xs sharp"><i class="fa fa-trash"></i></button>`
                            : `<button class="btn btn-danger shadow btn-xs sharp opacity-50" disabled><i class="fa fa-trash"></i></button>`
                    }
                </div>
                </td>
            `;
            DOM.tbody().appendChild(tr);
        });

        // Initialize DataTable
        $(DOM.table()).DataTable({
            lengthChange: true,
            searching: true,
            pageLength: 25,
            ordering: true,
            order: [[0, "desc"]], // Order by LR No Descending by default
            language: {
                paginate: {
                    next: '<i class="fa fa-angle-double-right" aria-hidden="true"></i>',
                    previous: '<i class="fa fa-angle-double-left" aria-hidden="true"></i>'
                }
            }
        });

    } catch (err) {
        console.error(err);
        showToast("danger", "Failed to load data", "LR Register");
    }
}

// ======================================================
// DELETE
// ======================================================
async function deleteLR(id) {
    const ok = await confirmDelete("This Lorry Receipt will be deleted permanently. Are you sure?");
    if (!ok) return;

    try {
        const res = await apiFetch(`${API}/delete/${id}`, { method: "DELETE" });
        const json = await res.json();

        if (!json.success) {
            throw new Error(json.message);
        }

        showToast("success", json.message, "LR Register");
        await bindTable(); // Refresh the grid seamlessly

    } catch (err) {
        showToast("danger", err.message, "LR Register");
    }
}