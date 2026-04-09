// ======================================================
// CONFIGURATION
// ======================================================
const API_URL = "/api/transaction/challan";

// ======================================================
// DOM ELEMENTS CACHE
// ======================================================
const DOM = {
    tbody: function () { return document.getElementById("tblBody"); },
    table: function () { return document.getElementById("challanListTable"); }
};

// ======================================================
// INITIALIZATION
// ======================================================
document.addEventListener("DOMContentLoaded", async function () {
    if (!guardPageAccess("Challan")) return;

    const btnCreate = document.getElementById("btnCreateNewChallan");

    if (btnCreate) {
        const hasAddRight = hasUserRight("Challan", "Add");
        btnCreate.classList.toggle("disabled", !hasAddRight);
        if (!hasAddRight) btnCreate.removeAttribute("href");
    }
    await bindChallanTable();
});

// ======================================================
// BIND TABLE DATA
// ======================================================
async function bindChallanTable() {
    try {
        // 1. Show a loading state in the grid
        DOM.tbody().innerHTML = '<tr><td colspan="8" class="text-center py-4">Loading data, please wait...</td></tr>';

        // 2. Fetch data from the API
        const response = await apiFetch(`${API_URL}/get-all`);
        const jsonResponse = await response.json();

        if (jsonResponse.success === false) {
            showToast("danger", jsonResponse.message, "Challan Register");
            return;
        }

        // 3. Destroy existing DataTable instance if it exists to prevent re-initialization errors
        if ($.fn.DataTable.isDataTable(DOM.table())) {
            $(DOM.table()).DataTable().destroy();
        }

        // 4. Clear the table body
        DOM.tbody().innerHTML = "";

        const canEdit = hasUserRight("Challan", "Update");
        const canDelete = hasUserRight("Challan", "Delete");

        // 5. Iterate through the data and build HTML rows
        const challanData = jsonResponse.data;

        for (let i = 0; i < challanData.length; i++) {
            const dataItem = challanData[i];

            // Format Date
            let displayDate = "-";
            if (dataItem.challanDate) {
                displayDate = new Date(dataItem.challanDate).toLocaleDateString('en-GB');
            }

            // Format Currency
            let finalAmountDisplay = "0.00";
            if (dataItem.finalAmt) {
                finalAmountDisplay = parseFloat(dataItem.finalAmt).toFixed(2);
            }

            // Handle NULLs gracefully for UI display
            const truckNumber = dataItem.truckNumber || "-";
            const consigneeName = dataItem.consigneeName || "-";
            const driverName = dataItem.driverName || "-";
            const entryBy = dataItem.e_By || "-";

            // Create the table row element
            const tr = document.createElement("tr");

            tr.innerHTML = `
                <td class="fw-bold text-primary text-center">${escapeHtml(dataItem.voucherNo || '-')}</td>
                <td class="text-center">${displayDate}</td>
                <td class="text-center fw-semibold">${escapeHtml(truckNumber)}</td>
                <td>${escapeHtml(consigneeName)}</td>
                <td>${escapeHtml(driverName)}</td>
                <td class="text-end fw-bold text-success">₹ ${finalAmountDisplay}</td>
                <td class="text-center">${escapeHtml(entryBy)}</td>
                <td class="text-center">
                    <div class="d-flex">
                    ${canEdit
                ? `<a href="/Transaction/ChallanEntry?id=${dataItem.idChallan}" class="btn btn-primary shadow btn-xs sharp me-1"><i class="fa fa-pencil"></i></a>`
                            : `<button class="btn btn-primary shadow btn-xs sharp me-1 opacity-50" disabled><i class="fa fa-pencil"></i></button>`
                        }

                            ${canDelete
                ? `<button onclick="deleteChallanRecord(${dataItem.idChallan})" class="btn btn-danger shadow btn-xs sharp"><i class="fa fa-trash"></i></button>`
                            : `<button class="btn btn-danger shadow btn-xs sharp opacity-50" disabled><i class="fa fa-trash"></i></button>`
                            }
                    </div>
                </td>
            `;

            // Append the row to the table body
            DOM.tbody().appendChild(tr);
        }

        // 6. Initialize DataTables
        $(DOM.table()).DataTable({
            lengthChange: true,
            searching: true,
            pageLength: 25,
            ordering: true,
            order: [[0, "desc"]], // Order by Voucher No Descending by default
            language: {
                paginate: {
                    next: '<i class="fa fa-angle-double-right" aria-hidden="true"></i>',
                    previous: '<i class="fa fa-angle-double-left" aria-hidden="true"></i>'
                }
            }
        });

    }
    catch (err) {
        console.error("Error binding Challan table:", err);
        showToast("danger", "An error occurred while loading the data.", "Challan Register");
    }
}

// ======================================================
// DELETE CHALLAN
// ======================================================
async function deleteChallanRecord(idChallan) {

    // 1. Confirm with the user
    const userConfirmed = await confirmDelete("This Challan and all its details will be deleted permanently. Are you sure?");

    if (userConfirmed === false) {
        return;
    }

    try {
        // 2. Call the Delete API
        const response = await apiFetch(`${API_URL}/delete/${idChallan}`, {
            method: "DELETE"
        });

        const jsonResponse = await response.json();

        // 3. Handle the response
        if (jsonResponse.success === false) {
            throw new Error(jsonResponse.message);
        }

        // 4. Show success message and reload the grid
        showToast("success", jsonResponse.message, "Challan Register");

        await bindChallanTable();

    }
    catch (err) {
        console.error("Error deleting Challan:", err);
        showToast("danger", err.message, "Challan Register");
    }
}