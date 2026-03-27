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
                    <div class="d-flex justify-content-center">
                        <a href="/Transaction/LREntry?id=${d.idlr}" class="btn btn-primary shadow btn-xs sharp me-1" title="Edit">
                            <i class="fa fa-pencil"></i>
                        </a>
                        <button type="button" onclick="deleteLR(${d.idlr})" class="btn btn-danger shadow btn-xs sharp" title="Delete">
                            <i class="fa fa-trash"></i>
                        </button>
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