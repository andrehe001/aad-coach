resource "kubernetes_namespace" "adventure_day_namespace" {
  metadata {
    name = "adventure-day"
  }
}

resource "kubernetes_namespace" "nginx_ingress" {
  metadata {
    name = "nginx-ingress"
  }
}

