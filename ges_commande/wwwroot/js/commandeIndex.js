document
  .getElementById("filterSelectPayer")
  .addEventListener("change", function () {
    const selectedValue = this.value;

    const url = new URL(window.location.href);

    if (selectedValue) {
      url.searchParams.set("IsPayer", selectedValue);
    } else {
      url.searchParams.delete("IsPayer"); // Supprimer le paramètre s'il est vide
    }

    // Rediriger vers la nouvelle URL
    window.location.href = url.toString();
  });

document
  .getElementById("filterSelectLivrer")
  .addEventListener("change", function () {
    const selectedValue = this.value;
    const url = new URL(window.location.href);

    if (selectedValue) {
      url.searchParams.set("Islivrer", selectedValue);
    } else {
      url.searchParams.delete("Islivrer"); // Supprimer le paramètre s'il est vide
    }

    window.location.href = url.toString();
  });
