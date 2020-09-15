resource "kubernetes_namespace" "monitoring" {
  metadata {
    annotations = {
      name = "monitoring"
    }

    name = "monitoring"
  }
}

resource "helm_release" "prometheus_operator" {
  name      = "po"
  chart     = "prometheus-operator"
  repository = "https://kubernetes-charts.storage.googleapis.com"
  namespace = kubernetes_namespace.monitoring.metadata[0].name

  depends_on = [helm_release.nginx_ingress]
}

resource "helm_release" "loki" {
  name      = "loki"
  chart     = "loki"
  repository = "https://grafana.github.io/loki/charts"
  namespace = kubernetes_namespace.monitoring.metadata[0].name

  depends_on = [helm_release.prometheus_operator]
}

resource "helm_release" "promtail" {
  name      = "promtail"
  chart     = "promtail"
  repository = "https://grafana.github.io/loki/charts"
  namespace = kubernetes_namespace.monitoring.metadata[0].name

  set {
    name  = "loki.serviceName"
    value = helm_release.loki.name
  }

  depends_on = [helm_release.loki]
}
