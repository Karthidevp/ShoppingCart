// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
<script type="text/javascript">
    $(document).ready(function() {
        // Retrieve messages from TempData
        var successMessage = '@TempData["success"]';
    var errorMessage = '@TempData["error"]';

    // Show success message if it exists
    if (successMessage) {
        toastr.success(successMessage);
        }

    // Show error message if it exists
    if (errorMessage) {
        toastr.error(errorMessage);
        }
    });
</script>
