"use strict";
document.addEventListener("DOMContentLoaded", function () {
  const btnLogin = document.getElementById("btn-open-login-dialog");
  btnLogin.addEventListener("click", (e) => {
    e.preventDefault();

    const loginDialog = document.createElement("dialog");

    loginDialog.classList.add("dialog");
    loginDialog.id = "login-dialog";
    document.body.appendChild(loginDialog);

    fetch(e.target.href)
      .then((response) => response.text())
      .then((html) => {
        loginDialog.innerHTML = html;
        loginDialog.showModal();

        const btnFormLogin = document.getElementById("btn-login-dialog");
        const btnGoRegist = document.getElementById("btn-regist");

        btnFormLogin.addEventListener("click", () => {
          const form = loginDialog.querySelector("form");
          const formData = new FormData(form);

          fetch("/Account/Login", {
            method: "POST",
            body: formData,
            headers: {
              "X-Requested-With": "XMLHttpRequest",
            },
          }).then((response) => {
            if (response.ok) {
              loginDialog.close();
                loginDialog.remove();
                location.reload();
            }
          });
        });
        btnGoRegist.addEventListener("click", (e) => {
          e.preventDefault();
          loginDialog.close();

          const registDialog = document.createElement("dialog");
          registDialog.classList.add("dialog");
          registDialog.id = "registration-dialog";
          document.body.appendChild(registDialog);
          fetch(e.target.href)
            .then((response) => response.text())
            .then((html) => {
              registDialog.innerHTML = html;
              registDialog.showModal();

              const btnRegistrate =
                document.getElementById("btn-regist-dialog");
              const btnBackLogin = document.getElementById("btn-back-to-login");

              btnRegistrate.addEventListener("click", () => {
                const form = registDialog.querySelector("form");
                const formData = new FormData(form);

                  fetch("/Account/Regist", {
                      method: "POST",
                      body: formData,
                      headers: {
                          "X-Requested-With": "XMLHttpRequest",
                      },
                  }).then((response) => {
                      if (response.ok) {
                          registDialog.close();
                          registDialog.remove();
                          alert("Your account has been created! You can login now!");
                          loginDialog.showModal();
                      }
                  });
              });

              btnBackLogin.addEventListener("click", () => {
                registDialog.close();
                registDialog.remove();
                loginDialog.showModal();
              });

              const btnRegistDialogClose = document.getElementById(
                "btn-close-regist-dialog"
              );
              btnRegistDialogClose.addEventListener("click", () => {
                registDialog.close();
                registDialog.remove();
                loginDialog.remove();
              });
            });
        });
        const btnLoginDialogClose = document.getElementById(
          "btn-close-login-dialog"
        );
        btnLoginDialogClose.addEventListener("click", () => {
          loginDialog.close();
          loginDialog.remove();
        });
      });
  });
});
