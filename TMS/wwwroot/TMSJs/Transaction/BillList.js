// ======================================================
// CONFIG
// ======================================================
const API = "/api/transaction/bill";

// ======================================================
// DOM CACHE
// ======================================================
const DOM = {
    tbody: () => document.getElementById("tblBody"),
    table: () => document.getElementById("billListTable")
};

// ======================================================
// INIT
// ======================================================
document.addEventListener("DOMContentLoaded", async () => {
    if (!guardPageAccess("Bill")) return;

    const btnCreate = document.getElementById("btnCreateNewBill");

    if (btnCreate) {
        const hasAddRight = hasUserRight("Bill", "Add");
        btnCreate.classList.toggle("disabled", !hasAddRight);
        if (!hasAddRight) btnCreate.removeAttribute("href");
    }

    await bindTable();
});

// ======================================================
// BIND TABLE
// ======================================================
async function bindTable() {
    try {
        // Show a loading message while fetching
        DOM.tbody().innerHTML = '<tr><td colspan="6" class="text-center">Loading data...</td></tr>';

        const res = await apiFetch(`${API}/get-all`);
        const json = await res.json();

        if (!json.success) {
            showToast("danger", json.message, "Bill Register");
            return;
        }

        // Destroy DataTable if it already exists to prevent initialization errors
        if ($.fn.DataTable.isDataTable(DOM.table())) {
            $(DOM.table()).DataTable().destroy();
        }

        DOM.tbody().innerHTML = "";

        json.data.forEach(d => {
            const tr = document.createElement("tr");

            // Format dates and currency
            const billDate = d.billDate ? new Date(d.billDate).toLocaleDateString('en-GB') : '-';
            const finalAmt = d.finalAmount ? parseFloat(d.finalAmount).toFixed(2) : "0.00";

            // This maps to the alias 'BillToPartyName' we defined in our SQL SP
            const partyName = d.billToPartyName || 'Unknown';

            const canEdit = hasUserRight("Bill", "Update");
            const canDelete = hasUserRight("Bill", "Delete");

            tr.innerHTML = `
                <td class="fw-bold text-primary">${escapeHtml(d.billNo || '-')}</td>
                <td>${billDate}</td>
                <td>${escapeHtml(partyName)}</td>
                <td class="fw-bold text-success">₹ ${finalAmt}</td>
                <td>${escapeHtml(d.e_By || '-')}</td>
                <td class="text-center">
                    <div class="d-flex justify-content-center">
                        ${canEdit
                        ? `<a href="/Transaction/BillEntry?id=${d.idBill}" class="btn btn-primary shadow btn-xs sharp me-1"><i class="fa fa-pencil"></i></a>`
                            : `<button class="btn btn-primary shadow btn-xs sharp me-1 opacity-50" disabled><i class="fa fa-pencil"></i></button>`
                            }

                            ${canDelete
                        ? `<button onclick="deleteBill(${d.idBill})" class="btn btn-danger shadow btn-xs sharp"><i class="fa fa-trash"></i></button>`
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
            order: [[0, "desc"]], // Order by Bill No Descending by default
            language: {
                paginate: {
                    next: '<i class="fa fa-angle-double-right" aria-hidden="true"></i>',
                    previous: '<i class="fa fa-angle-double-left" aria-hidden="true"></i>'
                }
            }
        });

    } catch (err) {
        console.error(err);
        showToast("danger", "Failed to load data", "Bill Register");
    }
}

// ======================================================
// DELETE
// ======================================================
async function deleteBill(id) {
    const ok = await confirmDelete("This Bill and its details will be deleted permanently. Are you sure?");
    if (!ok) return;

    try {
        const res = await apiFetch(`${API}/delete/${id}`, { method: "DELETE" });
        const json = await res.json();

        if (!json.success) {
            throw new Error(json.message);
        }

        showToast("success", json.message, "Bill Register");

        // Refresh the grid to remove the deleted row
        await bindTable();

    } catch (err) {
        showToast("danger", err.message, "Bill Register");
    }
}