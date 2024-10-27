/**
 * The Controls module provides utility functions for managing table content and modal popups.
 * - `RelaodTableBodyGet` updates a table body with data from a GET request.
 * - `RelaodTableBodyPost` submits a form via POST, hides a modal, and updates a table body.
 * - `OpenPopUpModal` loads and displays a modal with content fetched from a URL.
 */
var Controls = (function () {

    return {

        /**
         * Sends a GET request to the specified URL and updates the HTML content of the table body with the given ID.
         * Displays a loading spinner while the request is being processed.
         * @param {string} url - The URL to send the GET request to.
         * @param {string} tblBodyId - The ID of the table body to update.
         */
        RelaodTableBodyGet: function (url, tblBodyId) {

            // HTML for loading spinner to display while processing
            var loading = `<tr>
                                <td colspan="6">
                                        <div class="d-flex align-items-center justify-content-center gap-2">
                                            <div class="spinner-border text-primary my-3" role="status"></div>
                                            <p class="fs-5 m-0">Loading...</p>
                                        </div>
                                </td>
                            </tr>`;

            $.ajax({
                url: url,
                type: "GET",
                beforeSend: function () {
                    // Show loading spinner before sending the request
                    $(`#${tblBodyId}`).html(loading);
                },
                success: function (response) {
                    // Update table body with the response on success
                    $(`#${tblBodyId}`).html(response);
                },
                error: function () {
                    // Display an error message if the request fails
                    toastr.error("Something went wrong.");
                }
            });
        },

        /**
         * Sends a POST request using the provided form data, hides the specified popup, and reloads the table body.
         * Shows a success or error message based on the response.
         * @param {string} popUpId - The ID of the popup to hide after a successful request.
         * @param {jQuery} form - The jQuery form element containing the data to send in the POST request.
         * @param {string} tblBodyId - The ID of the table body to update.
         * @param {function} [afterSuccess] - Optional callback function to execute after a successful request.
         */
        RelaodTableBodyPost: function (popUpId, form, tblBodyId, afterSuccess) {

            // HTML for loading spinner to display while processing
            var loading = `<tr>
                                <td colspan="6">
                                        <div class="d-flex align-items-center justify-content-center gap-2">
                                            <div class="spinner-border text-primary my-3" role="status"></div>
                                            <p class="fs-5 m-0">Loading...</p>
                                        </div>
                                </td>
                            </tr>`;

            $.ajax({
                url: form.attr("action"),
                type: "POST",
                data: form.serialize(),
                success: function (response) {
                    if (response.success) {
                        // Hide the popup and show a success message
                        $(`#${popUpId}`).modal('hide');
                        toastr.success(response.message);
                        $(`#${tblBodyId}`).html(loading);

                        setTimeout(function () {
                            // Execute the callback or reload the page after success
                            if (afterSuccess)
                                afterSuccess();
                            else
                                location.reload();

                        }, 1000)
                    } else {
                        // Show an error message if the response indicates failure
                        toastr.error(response.message);
                    }
                },
                error: function () {
                    // Display an error message if the request fails
                    toastr.error("Something went wrong.");
                }
            });
        },

        /**
         * Opens a modal popup and populates it with content retrieved from the specified URL.
         * Sets the modal's title and button properties based on the provided arguments.
         * @param {string} url - The URL to fetch content for the popup.
         * @param {string} popUpId - The ID of the popup to display.
         * @param {string} title - The title to set for the popup modal.
         * @param {string} btnText - The text to display on the modal's submit button.
         * @param {string} btnClass - The CSS class to apply to the modal's submit button.
         */
        OpenPopUpModal: function (url, popUpId, title, btnText, btnClass) {
            $.get(url, function (data) {
                // Set the content, title, and button properties for the modal
                $(`#${popUpId} .modal-body`).html(data);
                $(`#${popUpId} .modal-title`).text(title);
                $(`#${popUpId} .btn-modal-submit`).text(btnText);
                $(`#${popUpId} .btn-modal-submit`).removeClass().addClass("btn btn-modal-submit " + btnClass);
                // Display the modal popup
                $(`#${popUpId}`).modal("show");
            });
        },

    };

})();
