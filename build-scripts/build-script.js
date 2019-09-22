// Import path module.
const path = require('path');
const fs = require('fs-extra');
const childProcess = require('child_process');

// List of files to be copied to distributed folder.
const copiedFiles = [
    {
        in: 'README.md',
        out: 'README.md'
    }
];
// myget.org api key.
const myGetApiKey = '139601cd-6b6c-48c0-a410-ae6bcbc2fc7e';

// Get projects folder.
const feedName = 'message-bus';
const projectName = 'LiteMessageBus';

// Get built project.
const projectFolder = path.resolve(projectsPath, projectName);

// Find distributed folder.
const distributedFolder = path.resolve(projectFolder, 'bin', 'Debug');

// Remove distributed folder.
if (fs.existsSync(distributedFolder)) {
    console.info(`Removing ${distributedFolder}`);
    childProcess.execSync(`rmdir /s /q ${distributedFolder}`);
    console.info(`${distributedFolder} has been removed`);
}

// Run ng build command.
console.info(`Building .Net Core project ${projectName}`);
childProcess.execSync(`dotnet pack ${projectFolder}/${projectName}.csproj`);
console.info(`Project ${projectName} has been built`);

// Distributed folder cannot be created.
if (!fs.existsSync(distributedFolder)) {
    console.error(`${distributedFolder} cannot be created. There must be something wrong. Please check again.`);
    return;
}

// Some files must be copied to distributed folder.
if (copiedFiles && copiedFiles.length) {
    for (const item of copiedFiles) {
        const sourceFile = path.resolve(projectFolder, item.in);
        const distributedFile = path.resolve(distributedFolder, item.out);

        console.log(`Trying to copy ${sourceFile} to ${distributedFile}`);
        if (fs.existsSync(sourceFile)) {
            console.info(`Copying ${sourceFile}...`);
            fs.copySync(sourceFile, distributedFile);
            console.info(`${sourceFile} has been copied`);
        }
    }
}

//#region Deployment

// Create .npmrc file.
console.log('Creating .npmrc file...');
const npmFile = path.resolve(distributedFolder, '.npmrc');
fs.writeFileSync(npmFile, `registry=https://www.myget.org/F/${projectName}/npm/
always-auth=true
//www.myget.org/F/${feedName}/npm/:_authToken=${myGetApiKey}
`);
console.log('Created .npmrc file');

// Deploy to myget hosting.
console.log(`Deploying ${projectName} to myget.org`);
childProcess.execSync('npm publish', { cwd: distributedFolder });
console.log(`Deployment done`);

//#endregion
