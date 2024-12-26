document.addEventListener("DOMContentLoaded", function () {
  const forms = document.querySelectorAll("[name='form_add_in_panier']");

  forms.forEach((form) => {
    form.addEventListener("submit", function (event) {
      event.preventDefault();

      const produitId = form.querySelector("input[name='produitId']").value;

      const data = new FormData();
      data.append("produitId", produitId);

      fetch("/api/addproducts", {
        method: "POST",
        body: data,
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error(`Erreur HTTP: ${response.status}`);
          }
          return response.json();
        })
        .then((data) => {
          if (data.success) {
            alert("Produit ajouté au panier!");
          } else {
            alert("Erreur lors de l'ajout du produit au panier.");
          }
        })
        .catch((error) => {
          console.error("Erreur:", error);
          alert("Une erreur est survenue: " + error.message);
        });
    });
  });
});

function deleteProduct(detailId) {
  fetch(`/Produit/DeleteProduit?detailId=${detailId}`, {
    method: "GET",
  })
    .then((response) => response.json())
    .then((data) => {
      if (data.success) {
        location.reload();
      } else {
        alert("Une erreur est survenue lors de la suppression");
      }
    })
    .catch((error) => {
      console.error("Erreur lors de la suppression:", error);
      alert("Une erreur est survenue");
    });
}

function updateQuantity(produitId, quantityChange) {
  const inputElement = document.getElementById(`bedrooms-input-${produitId}`);
  let newQuantity = Math.max(1, parseInt(inputElement.value) + quantityChange);
  inputElement.value = newQuantity;

  fetch(`/Produit/UpdateQuantity`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      produitId: produitId,
      quantity: newQuantity,
    }),
  })
    .then((response) => response.json())
    .then((data) => {
      if (data.success) {
        // Mettre à jour le prix du produit
        document.getElementById(
          `price-${produitId}`
        ).textContent = `${data.newMontant} FCFA`;

        // Mettre à jour le prix total
        document.getElementById(
          "invoiceTotal"
        ).textContent = `${data.total} FCFA`;
      } else {
        alert("Erreur lors de la mise à jour de la quantité");
      }
    })
    .catch((error) => {
      console.error("Erreur lors de la mise à jour de la quantité:", error);
      alert("Une erreur est survenue");
    });
}
