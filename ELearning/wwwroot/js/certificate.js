
document.addEventListener("DOMContentLoaded", function () {

    var certificateButton =
        document.querySelector(".certificate-button");

    var certificateModalElement =
        document.getElementById("certificateModal");

    var certificateContainer =
        document.getElementById("certificateContainer");

    var printButton =
        document.getElementById("printCertificateButton");


    if (!certificateButton || !certificateModalElement) {
        return;
    }


    // Bootstrap modal instance
    var certificateModal =
        new bootstrap.Modal(certificateModalElement);


    // Certificate button
    certificateButton.addEventListener("click", function () {

        var sid =
            certificateButton.getAttribute("data-sid");

        var userId =
            certificateButton.getAttribute("data-userid");


        // Show Bootstrap modal
        certificateModal.show();


        // Loading message
        certificateContainer.innerHTML =
            "<div class='text-center p-4'>" +
            "<div class='spinner-border text-primary' role='status'>" +
            "<span class='visually-hidden'>Loading...</span>" +
            "</div>" +
            "<p class='mt-3'>Loading certificate...</p>" +
            "</div>";


        // Certificate URL
        var url =
            "/MyCourses/Certificate" +
            "?sid=" + sid +
            "&userId=" + userId;


        fetch(url)

            .then(function (response) {

                if (!response.ok) {

                    throw new Error(
                        "Certificate cannot be generated."
                    );
                }

                return response.text();
            })

            .then(function (html) {

                certificateContainer.innerHTML = html;

            })

            .catch(function (error) {

                certificateContainer.innerHTML =
                    "<div class='alert alert-danger'>" +
                    error.message +
                    "</div>";

            });

    });


    // Print certificate
    if (printButton) {

        printButton.addEventListener("click", function () {

            var certificate =
                document.getElementById(
                    "certificatePrintArea"
                );


            if (!certificate) {

                alert(
                    "Certificate is not loaded yet."
                );

                return;
            }


            var printWindow =
                window.open("", "_blank");


            if (!printWindow) {

                alert(
                    "Please allow pop-ups in your browser."
                );

                return;
            }


            printWindow.document.open();


            printWindow.document.write(
                "<html>" +
                "<head>" +
                "<title>Certificate</title>" +

                "<style>" +

                "body {" +
                "margin: 0;" +
                "padding: 20px;" +
                "background: white;" +
                "text-align: center;" +
                "}" +

                "</style>" +

                "</head>" +

                "<body>" +

                certificate.outerHTML +

                "</body>" +

                "</html>"
            );


            printWindow.document.close();

            printWindow.focus();


            setTimeout(function () {

                printWindow.print();

                printWindow.close();

            }, 500);

        });

    }

});

