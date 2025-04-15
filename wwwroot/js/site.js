document.querySelectorAll("a").forEach((anchor) => {
  anchor.addEventListener("click", function (e) {
    const target = this.getAttribute("href");

    if (
      !target ||
      target.startsWith("/CreateAccount") ||
      target.startsWith("/Login") ||
      target.startsWith("/ForgottenPassword") ||
      target.startsWith("https") ||
      target.startsWith(".pdf") ||
      target.startsWith("/Cart") ||
      target.includes("Cart") ||
      target.includes("cart")
    ) {
      return;
    }

    e.preventDefault();

    fetch(target)
      .then((response) => response.text())
      .then((html) => {
        const parsedHTML = new DOMParser().parseFromString(html, "text/html");

        const content = parsedHTML.querySelector("main").innerHTML;
        document.querySelector("#content").innerHTML = content;

        window.history.pushState({}, "", target);

        const newTitle = parsedHTML.querySelector("title").innerText;
        document.title = newTitle;
      })
      .catch((err) => console.error("Error fetching page:", err));
  });
});

function openOtpModal() {
  const usernameElement = document.getElementById("Username");
  if (!usernameElement) {
    console.error("Username element not found");
    return;
  }

  let username = usernameElement.value;
  if (!username) {
    alert("Ange ditt användarnamn eller mobilnummer först.");
    return;
  }

  var loginModalEl = document.getElementById("loginModal");
  if (loginModalEl) {
    var loginModalInstance =
      bootstrap.Modal.getInstance(loginModalEl) ||
      new bootstrap.Modal(loginModalEl);
    loginModalInstance.hide();
  }

  var otpModalEl = document.getElementById("otpModal");
  if (otpModalEl) {
    var otpModalInstance = new bootstrap.Modal(otpModalEl);
    otpModalInstance.show();
  }
}

document.addEventListener("DOMContentLoaded", function () {
  const rememberMe = document.getElementById("rememberMe");

  if (rememberMe) {
    const username = document.getElementById("Username");
    const password = document.getElementById("Password");

    function toggleCheckbox() {
      if (!username || !password || !rememberMe) return;
      let isFilled =
        username.value.trim() !== "" && password.value.trim() !== "";
      rememberMe.disabled = !isFilled;
      if (!isFilled) {
        rememberMe.checked = false;
      }
    }

    if (username) username.addEventListener("input", toggleCheckbox);
    if (password) password.addEventListener("input", toggleCheckbox);
  }
});

document.addEventListener("DOMContentLoaded", function () {
  const otpContainer = document.getElementById("otp-container");

  if (!otpContainer) {
    return;
  }

  const verifyBtn = document.getElementById("verifyCodeBtn");
  const notFinished = document.getElementById("codeNotFinishedError");

  let otpInputs = [];

  for (let i = 0; i < 6; i++) {
    let input = document.createElement("input");
    input.type = "text";
    input.maxLength = 1;
    input.className = "otp-input form-control text-center";
    input.style.width = "40px";
    input.style.fontSize = "1.5em";
    otpContainer.appendChild(input);
    otpInputs.push(input);
    input.setAttribute("inputmode", "numeric");
    input.setAttribute("pattern", "[0-9]*");
  }

  otpInputs.forEach((input, index) => {
    input.addEventListener("input", function (e) {
      if (this.value.length === 1 && index < otpInputs.length - 1) {
        otpInputs[index + 1].focus();
      }

      checkOTPcompletion();
    });

    input.addEventListener("keydown", function (e) {
      if (e.key === "Backspace" && index > 0 && this.value.length === 0) {
        otpInputs[index - 1].focus();
      }
    });

    input.addEventListener("keypress", function (e) {
      if (!/[0-9]/.test(e.key)) {
        e.preventDefault();
      }
    });
  });

  function checkOTPcompletion() {
    let otpCode = otpInputs.map((input) => input.value).join("");
    verifyBtn.disabled = otpCode.length !== 6;

    let allEmpty = otpInputs.every((input) => input.value.trim() === "");
    if (allEmpty) {
      otpInputs.forEach((input) => input.classList.add("border-danger"));
      notFinished.style.display = "block";
    } else {
      otpInputs.forEach((input) => input.classList.remove("border-danger"));
      notFinished.style.display = "none";
    }
  }
});

function verifyBtnEnabled() {
  alert("Koden skickades till ditt telefonnummer!");
  const verifyBtn = document.getElementById("verifyBtn");
  if (verifyBtn) {
    verifyBtn.disabled = false;
  }
}

function placeOrder() {
  fetch("/Cart/PlaceOrder", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => {
      if (response.ok) {
        updateCartCount(0);
        window.location.href = "/Account/Profile";
      } else {
        return response.json().then((data) => {
          throw new Error(
            data.message || "Det gick inte att lägga beställningen"
          );
        });
      }
    })
    .catch((error) => {
      console.error("Error placing order:", error);
      const errorDiv = document.getElementById("orderErrorMessage");
      if (errorDiv) {
        errorDiv.textContent = error.message;
        errorDiv.style.display = "block";
      }
    });
}

function updateCartCount(count) {
  const cartCountElement = document.getElementById("cartCount");
  if (cartCountElement) {
    if (count > 0) {
      cartCountElement.textContent = count;
      cartCountElement.style.display = "inline-block";
    } else {
      cartCountElement.textContent = "0";
      cartCountElement.style.display = "none";
    }
  }
}
