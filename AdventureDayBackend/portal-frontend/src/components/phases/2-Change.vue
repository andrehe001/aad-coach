<template>
  <div>
    <div class="tablet">
      <img src="../../assets/images/phases/2-Task_small.jpg" />
    </div>
    <h2>Phase 2: Change</h2>
    <p>More and more support tickets are being displayed that relate to a feature that was already promised to the Smoorghs: higher stakes from humans. Slowly they are getting angry. We got the feedback from the World Game Federation, that it was already implemented, and they don’t know why it is not running in your team environment. Can you solve the problem?</p>
    <p>Goal: Fix the issue and add the missing feature,  if possible, without any downtime!</p>
    <h3>Hints</h3>
    <p>
      <a
        data-toggle="collapse"
        data-target="#collapseHints"
        aria-expanded="false"
        aria-controls="collapseHints"
        href="#"
      >Toggle Hints</a>
    </p>
    <ul class="collapse" id="collapseHints">
      <li>Always check if some functionality is not already implemented</li>
      <li>Ensure you have multiple instances running before doing updates, otherwise you will need maintenance windows</li>
    </ul>
    <h3>Helpful Resources</h3>
    <p>
      <a
        data-toggle="collapse"
        data-target="#collapseResources"
        aria-expanded="false"
        aria-controls="collapseResources"
        href="#"
      >Toggle Resources</a>
    </p>
    <ul class="collapse" id="collapseResources">
      <li>
        <a
          href="https://martinfowler.com/articles/feature-toggles.html"
        >Feature Toggles (aka Feature Flags)</a>
      </li>
      <li>
        <a
          href="https://kubernetes.io/docs/concepts/workloads/controllers/deployment/#rolling-update-deployment"
        >Kubernetes Rolling Update Deployment</a>
      </li>
      <li>
        <a
          href="https://kubernetes.io/docs/tasks/configure-pod-container/configure-liveness-readiness-startup-probes/"
        >Configure Liveness, Readiness and Startup Probes</a>
      </li>
    </ul>
  </div>
</template>