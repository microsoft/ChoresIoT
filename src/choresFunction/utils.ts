import {
  BlobServiceClient,
  BlockBlobClient,
  StorageSharedKeyCredential,
  BlobDownloadResponseModel,
} from "@azure/storage-blob";

import * as dotenv from "dotenv";
dotenv.config();

export function getBlob(): BlockBlobClient {
  const account = process.env.ACCOUNT_NAME || "";
  const accountKey = process.env.ACCOUNT_KEY || "";
  const containerName = process.env.CONTAINER_NAME || "";
  const blobName = process.env.BLOB_NAME || "";

  const sharedKeyCredential = new StorageSharedKeyCredential(
    account,
    accountKey
  );

  const blobServiceClient = new BlobServiceClient(
    `https://${account}.blob.core.windows.net`,
    sharedKeyCredential
  );

  const containerClient = blobServiceClient.getContainerClient(containerName);
  return containerClient.getBlockBlobClient(blobName);
}
