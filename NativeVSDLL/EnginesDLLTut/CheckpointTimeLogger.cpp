#include "CheckpointTimeLogger.h"

void CheckpointTimeLogger::ResetLogger()
{
	m_RTBC.clear();
	m_TRT = 0.0f;
}

void CheckpointTimeLogger::SaveCheckpointTime(float RTBC)
{
	std::ofstream myfile;
	myfile.open("Time.txt", std::ios::app);
	
	myfile << RTBC << "\n";

	myfile.close();

	m_RTBC.push_back(RTBC);

	m_TRT += RTBC;
}

float CheckpointTimeLogger::GetTotalTime()
{
	return m_TRT;
}

float CheckpointTimeLogger::GetCheckpointTime(int index)
{
	return m_RTBC[index];
}

int CheckpointTimeLogger::GetNumCheckpoints()
{
	return m_RTBC.size();
}
