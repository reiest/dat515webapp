#!/bin/sh
kubectl delete -f db-deployment.yaml
kubectl delete -f app-deployment.yaml
kubectl delete -f db-service.yaml
kubectl delete -f app-service.yaml
