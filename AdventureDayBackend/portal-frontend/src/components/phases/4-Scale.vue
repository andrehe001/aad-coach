<template>
  <div>
    <div class="tablet">
      <img src="../../assets/images/phases/4-Smorgsgram_small.jpg" />
    </div>
    <h2>Phase 4: Scale</h2>
    <p>Our plan is working - in the dark corners of Cybercity the news spread like wildfire: more and more Smoorghs are trying to play our game at the same time. But now our system is overloaded and slowly breaks down. Can you do the necessary scaling?</p>
    <p>Goal: Do the necessary scaling, so that the game remains playable, but we are not spending to much money!</p>
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
      <li>Changing manually the resources underneath is not the way to go</li>
      <li>Not all PaaS offerings are supporting automatic scaling off the shelf</li>
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
          href="https://docs.microsoft.com/en-us/azure/azure-sql/database/serverless-tier-overview"
        >Azure SQL Database serverless tier and autoscaling</a>
      </li>
      <li>
        <a
          href="https://docs.microsoft.com/en-us/azure/aks/concepts-scale"
        >Scaling options for applications in Azure Kubernetes Service (AKS)</a>
      </li>
      <li>
        <a
          href="https://docs.microsoft.com/en-us/azure/aks/tutorial-kubernetes-scale#autoscale-pods"
        >Scale applications in Azure Kubernetes Service (AKS)</a>
      </li>
    </ul>
  </div>
</template>