<template>
  <div>
    <div class="tablet">
      <img src="../../assets/images/phases/1-Message_small.jpg" />
    </div>
    <h2>Phase 1: Deployment</h2>
    <p>The Smoorghs love the game "Rock, Paper, Scissors, Metal, Snap" - so we should get it ready as soon as possible in order to buy our resources back.</p>
    <p>Goal: Get the game ready as soon as possible to earn virtual money from the Smoorghs!</p>
    <h3>Hints</h3>
    <p>
      <a
        data-toggle="collapse"
        data-target="#collapseHints"
        aria-expanded="false"
        aria-controls="collapseHints"
        href="#"
      >Toggle Hints (free for now...)</a>
    </p>
    <ul class="collapse" id="collapseHints">
      <li>
        World Game Fedaration has provided the source code to start with at:
        <a href="https://github.com/RicardoNiepel/azure-adventure-day-team">azure-adventure-day-team</a>
      </li>
      <li>
        Import the existing source with the
        <a
          href="https://docs.github.com/en/github/importing-your-projects-to-github/importing-a-repository-with-github-importer"
        >GitHub Importer</a> into your own public repository
      </li>
      <li>Take a close look into the existing sources, it may contain all necessary information regarding infrastructure and application deployment</li>
    </ul>
    <h3>Helpful Resources</h3>
    <p>
      <a
        data-toggle="collapse"
        data-target="#collapseResources"
        aria-expanded="false"
        aria-controls="collapseResources"
        href="#"
      >Toggle Resources (maybe you're strong enough without?)</a>
    </p>
    <ul class="collapse" id="collapseResources">
      <li>
        <a
          href="https://docs.microsoft.com/en-us/azure/developer/terraform/get-started-cloud-shell"
        >Configure Terraform using Azure Cloud Shell</a>
      </li>
      <li>
        <a
          href="https://docs.microsoft.com/en-us/azure/aks/kubernetes-walkthrough#connect-to-the-cluster"
        >Connect to an Azure Kubernetes Service (AKS) cluster and deploy the application</a>
      </li>
      <li>
        <a
          href="https://docs.github.com/en/actions/configuring-and-managing-workflows/configuring-a-workflow"
        >GitHub Actions for CI/CD workflows</a>
      </li>
    </ul>
  </div>
</template>
